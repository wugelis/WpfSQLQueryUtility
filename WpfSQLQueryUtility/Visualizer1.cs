using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WpfSQLQueryUtility
{
    // TODO: 將下列內容加入至 SomeType 的定義，以便在對 SomeType 的執行個體進行偵錯時看見此視覺化檢視:
    // 
    //  [DebuggerVisualizer(typeof(Visualizer1))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// SomeType 的視覺化檢視。
    /// </summary>
    public class Visualizer1 : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");

            // TODO: 取得要顯示其視覺化檢視的物件。
            //       將 objectProvider.GetObject() 的結果轉型為
            //       要視覺化的物件型別。
            object data = (object)objectProvider.GetObject();

            // TODO: 顯示您的物件檢視。
            //       以自己的自訂表單或控制項取代 displayForm。
            using (Form displayForm = new Form())
            {
                displayForm.Text = data.ToString();
                windowService.ShowDialog(displayForm);
            }
        }

        // TODO: 將下列內容加入至您的測試程式碼以測試視覺化檢視:
        // 
        //    Visualizer1.TestShowVisualizer(new SomeType());
        // 
        /// <summary>
        /// 在偵錯工具外部裝載視覺化檢視以便進行測試。
        /// </summary>
        /// <param name="objectToVisualize">要在視覺化檢視中顯示的物件。</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(Visualizer1));
            visualizerHost.ShowVisualizer();
        }
    }
}
