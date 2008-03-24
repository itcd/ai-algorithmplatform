using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Position_Interface;
using Position_Implement;
using PositionSetViewer;

[assembly: DebuggerVisualizer(typeof(PositionSetVisualizers.PositionSetVisualizer),
   Target = typeof(IPositionSet),
    Description = "PositionSet Visualizer")]
[assembly: DebuggerVisualizer(typeof(PositionSetVisualizers.PositionSetVisualizer),
   Target = typeof(PositionSet_ImplementByIEnumerable),
    Description = "PositionSet Visualizer")]
[assembly: DebuggerVisualizer(typeof(PositionSetVisualizers.PositionSetVisualizer),
   Target = typeof(PositionSetEdit_ImplementByICollectionTemplate),
    Description = "PositionSet Visualizer")]
[assembly: DebuggerVisualizer(typeof(PositionSetVisualizers.PositionSetVisualizer),
   Target = typeof(RandomPositionSet_Square),
    Description = "PositionSet Visualizer")]
namespace PositionSetVisualizers
{
    // TODO: Add the following to SomeType's definition to see this visualizer when debugging instances of SomeType:
    // 
    //  [DebuggerVisualizer(typeof(PositionSetVisualizer))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// A Visualizer for SomeType.  
    /// </summary>
    public class PositionSetVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            // TODO: Get the object to display a visualizer for.
            //       Cast the result of objectProvider.GetObject() 
            //       to the type of the object being visualized.
            IPositionSet data = (IPositionSet)objectProvider.GetObject();

            // TODO: Display your view of the object.
            //       Replace displayForm with your own custom Form or Control.

            //RandomPositionSet data = new RandomPositionSet(100, -1000, 1000);

            LayersExOptDlg Painter = new LayersExOptDlg();

            using (LayersPainterForm frmPainter = new LayersPainterForm(Painter))
            {
                //PainterDialog.DrawPositionSet(data);

                Layer lay = new Layer_PositionSet_Point(data);
                Painter.Add(lay);
                frmPainter.Show();
                frmPainter.Refresh();

                frmPainter.Visible = false;

                //displayForm.Text = data.ToString();
                windowService.ShowDialog(frmPainter);
            }
        }

        // TODO: Add the following to your testing code to test the visualizer:
        // 
        //    PositionSetVisualizer.TestShowVisualizer(new SomeType());
        // 
        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(PositionSetVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}