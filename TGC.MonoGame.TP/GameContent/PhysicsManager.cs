

using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using TGC.MonoGame.Samples.Physics.Bepu;

public class PhysicsManager
{
    private static PhysicsManager _instance;
    private Simulation _simulation;
    private BufferPool _buffer;
    private SimpleThreadDispatcher _threadDispatcher;

    private PhysicsManager()
    {
        _buffer = new BufferPool();
        _simulation = Simulation.Create(
            _buffer,
            new NarrowPhaseCallbacks(new SpringSettings(30,1)),
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
    }
    public TypedIndex AddShape(Box shape) => _simulation.Shapes.Add(shape);
    public TypedIndex AddShapeSphere(Sphere shape) => _simulation.Shapes.Add(shape);

    public BodyHandle AddBody(BodyDescription body) => _simulation.Bodies.Add(body);
    public BodyReference GetBodyReference(BodyHandle bodyHandle)
        => _simulation.Bodies.GetBodyReference(bodyHandle);
}