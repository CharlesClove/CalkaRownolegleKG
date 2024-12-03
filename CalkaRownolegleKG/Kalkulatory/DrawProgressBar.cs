namespace CalkaRownolegleKG.Kalkulatory
{
    public class ProgressBar
    {
        private int taskId;
        private int totalTasks;
        private int lastProgress = -1; // do sledzenia procentow
        public ProgressBar(int taskId, int totalTasks)
        {
            this.taskId = taskId;
            this.totalTasks = totalTasks;
            
        }
        public void DrawProgress(int current, int total)
        {
            int progressWidth = 50;
            int progressProcenty = (int)((double)current / total * 100);
            if (progressProcenty / 10 > lastProgress)
            {
                lastProgress = progressProcenty / 10;
                int filledWidth = (int)((double)current / total * progressWidth);
                lock (Console.Out)
                {
                    Console.SetCursorPosition(0, Math.Min(taskId, Console.WindowHeight - 1));
                    Console.Write($"Task {taskId + 1}/{totalTasks}: [");
                    Console.Write(new string('#', filledWidth));
                    Console.Write(new string(' ', progressWidth - filledWidth));
                    Console.Write($"] {current}/{total}");
                    Console.Write($"] {progressProcenty}%");
                    Thread.Sleep(100);

                }
            }

        }

    }
}
