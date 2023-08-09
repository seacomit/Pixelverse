using Assets.Core.Archetypes;
using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Rendering;
using Assets.Core.Voxels.Compute.Math.Elements;
using Assets.Core.Voxels.Compute.Math.Ops;
using UnityEngine;
using Assets.Core.Voxels.Compute.Shaders;

namespace Assets.Core
{
    public class ComputeShaderTest : MonoBehaviour
    {
        public ComputeShader computeShader;
        public ComputeShader transformComputeShader;
        public ComputeShader rendererComputeShader;
        public ComputeShader mathComputeShader;

        public SpriteRenderer spriteRenderer;
        private VoxelCompute forge;
        private VoxelSet voxelSet;
        private VoxelRenderer voxelRenderer;
        private float angle = 0;
        private const int defaultVoxelBufferSize = 128;
        public Vector4 centeredOffset;
        public Vector3 staticRotation;
        UnityEngine.Rendering.GraphicsFence fence;

        private PointBuffer line;

        // Start is called before the first frame update
        void Start()
        {
            centeredOffset = new Vector4(defaultVoxelBufferSize / 2.0f, defaultVoxelBufferSize / 2.0f, defaultVoxelBufferSize / 2.0f, 0);
            staticRotation = new Vector3(0, 0, 0);
            voxelSet = VoxelConfiguration.build();
            forge = new VoxelCompute(computeShader, transformComputeShader, mathComputeShader);
            voxelRenderer = new RealisticVoxelRenderer(rendererComputeShader, defaultVoxelBufferSize);
            //voxelRenderer = new StyledVoxelRenderer(rendererComputeShader, defaultVoxelBufferSize);
            voxelRenderer.initialize();
            fence = Graphics.CreateGraphicsFence(UnityEngine.Rendering.GraphicsFenceType.CPUSynchronisation, UnityEngine.Rendering.SynchronisationStageFlags.ComputeProcessing);

            /*Vector4[] linePoints = new Vector4[defaultVoxelBufferSize / 2];
            for (int i = 0; i < defaultVoxelBufferSize / 2; i++)
            {
                linePoints[i] = new Vector4(0, 0, i - 64 / 2);
            }*/
            line = PointBuffer.Cache.GetOrBuild(defaultVoxelBufferSize / 2);
            //line.buffer().SetData(linePoints);
        }

        // Update is called once per frame
        void Update()
        {
            if (fence.passed)
            {
                angle += 0.5f;
                rotatingGeometry();
                //rotatingCopperSword();
            }
        }

        private void rotatingCopperSword()
        {
            // Super Abstracts
            Archetypes.Component hiltComponent = new Archetypes.Component();
            hiltComponent.id = "hilt";
            Archetypes.Component guardComponent = new Archetypes.Component();
            guardComponent.id = "guard";
            Archetypes.Component bladeComponent = new Archetypes.Component();
            guardComponent.id = "blade";
            bladeComponent.properties.Add("Length", ComponentProperty.newIntRange(2, 50));
            bladeComponent.properties.Add("Width", ComponentProperty.newRelativeIntRange("Length", 5, 10));
            bladeComponent.properties.Add("Depth", ComponentProperty.newRelativeIntRange("Width", 1, 25));
            // Blade actually consists of multiple parts.
            // Tip, Core, and Edge. If each is defined, it makes it easier to determine the rendered geometric parts.

            // Constrained Abstracts
            Archetypes.Component swordBladeComponent = new Archetypes.Component();
            swordBladeComponent.baseComponent = bladeComponent;
            swordBladeComponent.id = "swordBlade";
            swordBladeComponent.properties.Add("Length", ComponentProperty.newIntRange(10, 50));

            Socket hiltSocket = new Socket();
            Socket guardSocket = new Socket();
            Socket bladeSocket = new Socket();

            Connection hiltToGuardConnection = new Connection();
            hiltToGuardConnection.component = hiltComponent;
            hiltToGuardConnection.orientation = Vector3.up;
            hiltToGuardConnection.socket = guardSocket;
            hiltSocket.connections.Add(hiltToGuardConnection);

            Connection guardToBladeConnection = new Connection();
            guardToBladeConnection.component = guardComponent;
            guardToBladeConnection.orientation = Vector3.up;
            guardToBladeConnection.socket = bladeSocket;
            guardSocket.connections.Add(guardToBladeConnection);

            Connection bladeToNilConnection = new Connection();
            bladeToNilConnection.component = swordBladeComponent;
            bladeToNilConnection.orientation = Vector3.up;
            bladeSocket.connections.Add(bladeToNilConnection);

            Archetype sword = new Archetype();
            sword.id = "sword";
            sword.rootSocket = hiltSocket;

            // Implementation
            Archetype copperSword = new Archetype();
            copperSword.id = "copperSword";
            
        }

        private void rotatingGeometry()
        {
            Identity voxels = new Identity();

            Geometry.Tetrahedron geoTetra = new Geometry.Tetrahedron(32);
            Tetrahedron tetra = new Tetrahedron(
                    new VoxelTransform(
                        voxelSet.voxels[2].type,
                        centeredOffset - geoTetra.getCenteredOffset(),
                        new Vector3(32, 32, 32),
                        Quaternion.Euler(angle, angle, angle),
                        centeredOffset - geoTetra.getCenteredOffset()));

            Geometry.Triangle geoTriangle = new Geometry.Triangle(
                new Vector3(0, 1), new Vector3(1, -1), new Vector3(-1, -1));
            Triangle triangle = new Triangle(
                    geoTriangle,
                    new VoxelTransform(
                        voxelSet.voxels[2].type,
                        centeredOffset - geoTriangle.scale(16).getCenteredOffset(),
                        new Vector3(16, 1),
                        Quaternion.identity,//Quaternion.Euler(angle, angle, angle),
                        Vector3.zero));//centeredOffset - geoTriangle.scale(16).getCenteredOffset()));

            Sphere sphere = new Sphere(
                new VoxelTransform(
                    voxelSet.voxels[4].type,
                    centeredOffset / 5,
                    new Vector3(16, 16, 16),
                    Quaternion.Euler(-angle, angle, -angle),
                    centeredOffset)
                );

            Cube cube = new Cube(
                new VoxelTransform(
                    voxelSet.voxels[3].type,
                    centeredOffset + centeredOffset / 5,
                    new Vector3(32, 32, 32),
                    Quaternion.Euler(-angle, -angle, angle),
                    centeredOffset));

            Rotate triRotate = 
                new Rotate(new VoxelTransform(
                        0,
                        Vector3.zero,
                        Vector3.zero,
                        Quaternion.Euler(angle, angle, angle),
                        centeredOffset - geoTriangle.scale(16).getCenteredOffset()), 
                new Multiply(triangle, line));

            Add add = new Add(new Add(new Add(
                voxels, triRotate),
                sphere),
                cube);

            VoxelBuffer finalAddition = add.evaluate();
            voxelRenderer.render(finalAddition.buffer(), spriteRenderer);
            finalAddition.Release();
        }

        private void OnDestroy()
        {
            forge.cleanup();
            voxelRenderer.cleanup();
            VoxelBuffer.Cache.Release();
            PointBuffer.Cache.Release();
        }
    }
}
