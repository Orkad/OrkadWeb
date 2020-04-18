namespace OrkadWeb.Services
{
    public interface ISupervisionService
    {
        /// <summary>
        /// Récupère la temperature courante du processeur en degrès
        /// </summary>
        double GetCpuTemperature();
    }
}