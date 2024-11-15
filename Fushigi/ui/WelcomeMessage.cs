using Fushigi.ui.modal;
using ImGuiNET;

namespace Fushigi.ui
{
    public partial class MainWindow
    {
        class WelcomeMessage : OkDialog<WelcomeMessage>
        {
            protected override string Title => "Welcome";

            protected override void DrawBody()
            {
                ImGui.Text("Welcome to Fushigi! Set the RomFS game path and save directory to get started.");
            }
        }
    }
}
