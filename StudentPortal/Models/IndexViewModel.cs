namespace StudentPortal.Models
{
    public class IndexViewModel
    {
        public List<Entities.Students> Students { get; set; }
        public List<Entities.Schedules> Schedules { get; set; }
        public List<Entities.Subjects> Subjects { get; set; }
    }
}
