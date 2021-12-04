// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Ugao rotacije Meseca
        /// </summary>
        private float m_moonRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije Zemlje
        /// </summary>
        private float m_earthRotation = 0.0f;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 900.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        private int window_width;

        private int window_height;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public int WindowWidth
        {
            get { return window_width; }
            set { window_width = value; }
        }

        public int WindowHeight
        {
            get { return window_height; }
            set { window_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.4f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            m_scene.LoadScene();
            m_scene.Initialize();
        }

        public void DrawBall(OpenGL gl)
        {
            gl.PushMatrix(); 
            gl.Translate(0.0f, -35f, 0f);

            m_scene.Draw();

            gl.PopMatrix();
        }

        public void DrawSurface(OpenGL gl)
        {
            gl.PushMatrix();

            gl.Color(0.39f, 0.69f, 0.37f);

            gl.Translate(0.0f, 90f, -0);
            gl.Rotate(4f, 0f, 0f);

            gl.Begin(OpenGL.GL_QUADS);

            gl.Normal(0f, 1f, 0f);
            gl.Vertex4f(450f, -100f, 1300, 1);
            gl.Vertex4f(450f, -100f, -800, 1);
            gl.Vertex4f(-450f, -100f, -800, 1);
            gl.Vertex4f(-450f, -100f, 1300, 1);

            gl.End();

            gl.PopMatrix();
        }

        public void DrawPosts(OpenGL gl)
        {
            //right post
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(300f, 405f, -150f);
            gl.Rotate(90f, 0f, 0f);


            Cylinder rightPost = new Cylinder();
            rightPost.BaseRadius = 10;
            rightPost.TopRadius = 10;
            rightPost.Height = 400;

            rightPost.CreateInContext(gl);
            rightPost.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
            //-right post

            //left post
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(-300f, 405f, -150f);
            gl.Rotate(90f, 0f, 0f);

            Cylinder leftPost = new Cylinder();

            leftPost.BaseRadius = 10;
            leftPost.TopRadius = 10;
            leftPost.Height = 400;

            leftPost.CreateInContext(gl);
            leftPost.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
            //-left post
        }

        public void DrawCrossbars(OpenGL gl)
        {
            //crossbar
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(-300, 400f, -150);
            gl.Rotate(90f, 90f, 0f);

            Cylinder CrossbarTop = new Cylinder();
            CrossbarTop.BaseRadius = 10;
            CrossbarTop.TopRadius = 10;
            CrossbarTop.Height = 610;

            CrossbarTop.CreateInContext(gl);
            CrossbarTop.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            //-crossbar

            //crossbar back
            gl.PushMatrix();

            gl.Color(0.8f, 0.8f, 0.8f);
            gl.Translate(-300, 40f, -355);

            gl.Rotate(90f, 90f, 0f);

            Cylinder crossbarBack = new Cylinder();

            crossbarBack.BaseRadius = 7;
            crossbarBack.TopRadius = 7;
            crossbarBack.Height = 400;

            crossbarBack.CreateInContext(gl);
            CrossbarTop.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);


            gl.PopMatrix();

            //-crossbar back
        }

        public void DrawBeams(OpenGL gl)
        {
            //left beam
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(-300f, 390.0f, -152f);
            gl.Rotate(120f, 0.3f, 0.4f);

            Cylinder leftBeam = new Cylinder();

            leftBeam.BaseRadius = 7;
            leftBeam.TopRadius = 7;
            leftBeam.Height = 410;

            leftBeam.CreateInContext(gl);
            leftBeam.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);


            gl.PopMatrix();
            //-left beam

            //right beam
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(300f, 390.0f, -152f);
            gl.Rotate(120f, 0.3f, 0.4f);

            Cylinder rightBeam = new Cylinder();

            rightBeam.BaseRadius = 7;
            rightBeam.TopRadius = 7;
            rightBeam.Height = 410;

            rightBeam.CreateInContext(gl);
            rightBeam.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
            //-right beam

        }

        public void DrawBases(OpenGL gl)
        {
            //left base
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(-298f, 40f, -355f);
            gl.Rotate(190f, 182f, 11f);

            Cylinder leftBase = new Cylinder();

            leftBase.BaseRadius = 7;
            leftBase.TopRadius = 7;
            leftBase.Height = 206;

            leftBase.CreateInContext(gl);
            leftBase.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
            //-left base

            //right base
            gl.PushMatrix();

            gl.Color(0.8f, 0.8f, 0.8f);

            gl.Translate(305f, 40f, -355f);
            gl.Rotate(190f, 182f, 11f);

            Cylinder rightBase = new Cylinder();

            rightBase.BaseRadius = 7;
            rightBase.TopRadius = 7;
            rightBase.Height = 206;

            rightBase.CreateInContext(gl);
            rightBase.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
            //-left base
        }

        public void DrawText(OpenGL gl)
        {
            gl.PushMatrix();
            gl.Color(0.0f, 0.0f, 0.0f);
            gl.Viewport(m_width / 2, m_height, m_width, m_height);

            gl.DrawText3D("Tahoma", 10, 0, 0, "");
            gl.DrawText(WindowWidth - 165, WindowHeight - 70, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "Predmet: Racunarska grafika");
            gl.DrawText(WindowWidth - 165, WindowHeight - 72, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "___________________________");
            gl.DrawText(WindowWidth - 150, WindowHeight - 85, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "Sk.god: 2021/22.");
            gl.DrawText(WindowWidth - 150, WindowHeight - 87, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "________________");
            gl.DrawText(WindowWidth - 150, WindowHeight - 100, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "Ime: Filip");
            gl.DrawText(WindowWidth - 150, WindowHeight - 102, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "___________");
            gl.DrawText(WindowWidth - 150, WindowHeight - 115, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "Prezime: Pinjuh");
            gl.DrawText(WindowWidth - 150, WindowHeight - 117, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "________________");
            gl.DrawText(WindowWidth - 150, WindowHeight - 130, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "Sifra zad: 17.1");
            gl.DrawText(WindowWidth - 150, WindowHeight - 132, 0.0f, 0.0f, 0.0f, "Tahoma", 10, "________________");

            gl.PopMatrix();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.PushMatrix();
            gl.Translate(0.0f, 0.0f, -m_sceneDistance);
            gl.Scale(0.5, 0.5, 0.5);

            gl.Rotate(m_xRotation, 2.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            gl.Color(0.3f, 0.1f, 0.99f);

            DrawBall(gl);

            DrawSurface(gl);

            DrawPosts(gl);

            DrawCrossbars(gl);

            DrawBases(gl);

            DrawBeams(gl);

            DrawText(gl);

            gl.PopMatrix();

            // Oznaci kraj iscrtavanja
            gl.Flush();
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(45f, (double)width / height, 0.5f, 25000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
