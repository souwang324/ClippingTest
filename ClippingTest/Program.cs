using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitware.VTK;

namespace ClippingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ClippingTest1();
        }
        public static void ClippingTest1()
        {
            vtkSTLReader pSTLReader = vtkSTLReader.New();
            pSTLReader.SetFileName("../../../../res/cow.stl");
            pSTLReader.Update();

            double[] center = new double[3];
            center = pSTLReader.GetOutput().GetCenter();

            vtkPlane plane = vtkPlane.New();
            plane.SetOrigin(center[0], center[1], center[2]);
            plane.SetNormal(1.0, 0.0, 0.0);

            vtkClipPolyData clipper = vtkClipPolyData.New();
            clipper.SetInputConnection(pSTLReader.GetOutputPort());
            clipper.SetClipFunction(plane);
            clipper.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(clipper.GetOutputPort());

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);

            vtkRenderer renderer = vtkRenderer.New();
            renderer.AddActor(actor);
            renderer.SetBackground(.1, .2, .3);
            renderer.ResetCamera();

            vtkRenderWindow renderWin = vtkRenderWindow.New();
            renderWin.AddRenderer(renderer);

            vtkRenderWindowInteractor interactor = vtkRenderWindowInteractor.New();
            interactor.SetRenderWindow(renderWin);

            renderWin.Render();
            interactor.Start();
        }
    }
}
