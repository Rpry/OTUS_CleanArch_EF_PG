namespace Domain.Entities
{
    /// <summary>
    /// Урок (вью).
    /// </summary>
    public class ViewLesson: IEntity<int>
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary> Тема. </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Удалено.
        /// </summary>
        public bool Deleted { get; set; }
    }
}