using UnityEngine;

namespace ModelImporter.Editor.Windows
{
    public class WrongPositionOrRotationWindow : ModelImporterBaseDialogWindow<WrongPositionOrRotationWindow>
    {
        private const int Width = 500;
        private const int Height = 150;
        
        protected override string WindowTitle { get { return "Wrong Position or Rotation of Model"; } }
        
        protected override void DrawContentEditor()
        {
            _size = new Vector2(Width, Height);
            base.DrawContentEditor();
        }
    }
}