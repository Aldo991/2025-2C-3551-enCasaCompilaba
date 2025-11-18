

using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using TGC.MonoGame.Samples.Physics.Bepu;
using TGC.MonoGame.TP;

public class PhysicsManager
{
    private static PhysicsManager _instance;
    private Simulation _simulation;
    private BufferPool _buffer;
    private SimpleThreadDispatcher _threadDispatcher;
    private NarrowPhaseCallbacks _narrowPhaseCallbacks;
    private Dictionary<BodyHandle, GameObject> _bodyToEntity;

    private PhysicsManager()
    {
        _buffer = new BufferPool();
        _narrowPhaseCallbacks = new NarrowPhaseCallbacks(new SpringSettings(30,1));
        _bodyToEntity = new Dictionary<BodyHandle, GameObject>();
        _simulation = Simulation.Create(
            _buffer,
            _narrowPhaseCallbacks,
            new PoseIntegratorCallbacks(new System.Numerics.Vector3(0,0f,0)),
            // new PoseIntegratorCallbacks(new System.Numerics.Vector3(0,-9.8f,0)),
            new SolveDescription(8,1)
        );
        var targetThreadCount = Math.Max(1,
            Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
        _threadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
        /*
        _simulation.Statics.Add(
            new StaticDescription(
                new System.Numerics.Vector3(0, -0.5f, 0),
                _simulation.Shapes.Add(new Box(2000, 1, 2000))
            )
        );
        */

    }
    // La hago singleton, para que solo haya una única instancia de esta clase
    public static PhysicsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PhysicsManager();
            return _instance;
        }
    }
    public void Update()
    {
        _simulation.Timestep(1 / 60f, _threadDispatcher);
        ProcessCollisionEvents();
    }
    public TypedIndex AddShape(Box shape) => _simulation.Shapes.Add(shape);
    public TypedIndex AddShapeSphere(Sphere shape) => _simulation.Shapes.Add(shape);
    public BodyHandle AddBody(BodyDescription body, GameObject gameObject)
    {
        var bodyHandle = _simulation.Bodies.Add(body);
        _bodyToEntity.Add(bodyHandle, gameObject);
        return bodyHandle;
    }
    public BodyReference GetBodyReference(BodyHandle bodyHandle)
        => _simulation.Bodies.GetBodyReference(bodyHandle);
    private void ProcessCollisionEvents()
    {
        var seen = new HashSet<(BodyHandle, BodyHandle)>();
        while (_narrowPhaseCallbacks.CollisionQueue.TryDequeue(out var evt))
        {
            var pair = evt.A.Value < evt.B.Value ? (evt.A, evt.B) : (evt.B, evt.A);
            if (!seen.Add(pair)) continue;

            if (!_bodyToEntity.TryGetValue(evt.A, out var entA)) entA = null;
            if (!_bodyToEntity.TryGetValue(evt.B, out var entB)) entB = null;

            if (entA is Tank && entB is Projectile)
                HandleProjectileHit((Tank) entA, (Projectile) entB, evt);
            else if (entB is Tank && entA is Projectile)
                HandleProjectileHit((Tank) entA, (Projectile) entB, evt);
        }
    }
    private void HandleProjectileHit(Tank tank, Projectile projectile, CollisionEvent collisionEvent)
    {
        tank.CambiarVida(-10);
        _simulation.Bodies.Remove(projectile.GetBodyHandle());
        _bodyToEntity.Remove(projectile.GetBodyHandle());
        projectile.Desactivate();
        GameManager.RemoveProjectileFromProjectileManager(projectile);
        if (tank.GetLife() <= 0 && !tank.GetIsPlayer())
            GameManager.RemoveTankFromTankManager(tank);
    }
}