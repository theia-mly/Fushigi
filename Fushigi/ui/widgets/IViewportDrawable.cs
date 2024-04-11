using ImGuiNET;

namespace Fushigi.ui.widgets
{
    interface IViewportDrawable
    {
        void Draw2D(CourseAreaEditContext editContext, LevelViewport viewport, ImDrawListPtr dl, ref bool isNewHoveredObj);
    }
}
