
namespace YandexUploadFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartProccess();
        }

        private static void StartProccess()
        {
            IWorkflow wf = new Workflow();
            wf.TemplateWork();
        }

    }
}
