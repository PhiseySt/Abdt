namespace YandexUploadFiles
{
    internal interface IWorkflow
    {
        // pattern template method
        void FilesWork();
        void UserWork();
        void UploadWork();

        // c# 8.0 feature body method in interface
        public void TemplateWork()
        {
            FilesWork();
            UserWork();
            UploadWork();
        }
    }
}
