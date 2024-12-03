namespace CalkaRownolegleKG.Kalkulatory
{
    public class ProgressBar
    {
        private int taskId;
        private int totalTasks;
        public ProgressBar(int taskId, int totalTasks)
        {
            this.taskId = taskId;
            this.totalTasks = totalTasks;
        }
        public void DrawProgress(int current, int total)
        {
            int progressWidth = 50;
            int filledWidth = (int)((double)current / total * progressWidth);
            lock (Console.Out)
            {
                Console.SetCursorPosition(0, Math.Min(taskId, Console.WindowHeight - 1));
                Console.Write($"Task {taskId + 1}/{totalTasks}: [");
                Console.Write(new string('#', filledWidth));
                Console.Write(new string(' ', progressWidth - filledWidth));
                Console.Write($"] {current}/{total}");
                //Thread.Sleep(1);

            }

        }



    }
}
