namespace AdaptationManagement
{
    public class AdaptationAnalysisData
    {
        public string Department { get; set; }
        public string Position { get; set; }
        public string Quarter { get; set; }
        public int TotalPrograms { get; set; }
        public int CompletedPrograms { get; set; }
        public int ErrorsCount { get; set; }
        public int EmployedAfterProgram { get; set; }

        public double CompletionPercentage => (double)CompletedPrograms / TotalPrograms * 100;
        public double EmploymentRate => (double)EmployedAfterProgram / TotalPrograms * 100;
    }
}