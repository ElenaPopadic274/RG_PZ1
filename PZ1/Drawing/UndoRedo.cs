using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ1.Drawing
{
    public enum ActionType
    {
        CreateShape = 0,
        DeleteShape = 1,
        ClearCanvas = 2,
        NoActon = 3
    }
    class UndoRedo : IUndoRedo
    {
        private List<FrameworkElement> UndoShape = new List<FrameworkElement>();
        private List<FrameworkElement> UndoClearShapes = new List<FrameworkElement>();
        private List<FrameworkElement> RedoShape = new List<FrameworkElement>();
        private ActionType actionType;

        public void InsertAllShapeforUndoRedo(List<FrameworkElement> shapesForUndoRedo)
        {
            foreach (var shape in shapesForUndoRedo)
            {
                UndoClearShapes.Add(shape);
            }
            actionType = ActionType.ClearCanvas;
        }

        public void InsertShapeforUndoRedo(FrameworkElement dataobject)
        {
            UndoShape.Add(dataobject);
            actionType = ActionType.CreateShape;
        }

        public void Redo(MainWindow mainWindow)
        {
            if (RedoShape.Count() != 0)
            {
                mainWindow.mapCanvas.Children.Add(RedoShape[RedoShape.Count() - 1]);
                mainWindow.ListAllShapes.Add(RedoShape[RedoShape.Count() - 1]);
                UndoShape.Add(RedoShape[RedoShape.Count() - 1]);
                RedoShape.Remove(RedoShape[RedoShape.Count() - 1]);
            }
        }

        public void Undo(MainWindow mainWindow)
        {
            if (actionType == ActionType.ClearCanvas && UndoClearShapes.Count() != 0)
            {
                foreach (var shape in UndoClearShapes)
                {
                    mainWindow.mapCanvas.Children.Add(shape);
                    mainWindow.ListAllShapes.Add(shape);
                }
                UndoClearShapes.Clear();
                actionType = ActionType.NoActon;
            }
            else
            {
                if (UndoShape.Count() != 0)
                {
                    mainWindow.mapCanvas.Children.Remove(UndoShape[UndoShape.Count() - 1]);
                    mainWindow.ListAllShapes.Remove(UndoShape[UndoShape.Count() - 1]);
                    RedoShape.Add(UndoShape[UndoShape.Count() - 1]);
                    UndoShape.Remove(UndoShape[UndoShape.Count() - 1]);
                }
            }
        }
    }
}
