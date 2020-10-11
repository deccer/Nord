namespace Nord.Client.UI
{
    public static class DrawText
    {
        public static void Perform(string text)
        {
            ImGuiNET.ImGui.Text(text);
        }
    }
}
