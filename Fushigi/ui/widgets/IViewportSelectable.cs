using Fushigi.course;
using ImGuiNET;

namespace Fushigi.ui.widgets
{
    interface IViewportSelectable
    {
        void OnSelect(CourseAreaEditContext editContext);
        public static void DefaultSelect(CourseAreaEditContext ctx, object selectable)
        {
            if (ImGui.GetIO().KeyShift || ImGui.GetIO().KeyCtrl)
            {
                ctx.Select(selectable);
            }
            else if(!ctx.IsSelected(selectable))
            {
                ctx.WithSuspendUpdateDo(() =>
                {
                    ctx.DeselectAll();
                    ctx.Select(selectable);
                });
            }
            foreach(CourseActor act in ctx.GetSelectedObjects<CourseActor>())
            {
                act.mStartingTrans = act.mTranslation;
            }
        }
    }
}
