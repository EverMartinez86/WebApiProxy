

using System.Linq.Expressions;

namespace Proyecto.Model.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Buscar por medio de la llave primaria de la entidad
        /// </summary>
        /// <param name="ValueKey">Llave primaria</param>
        /// <returns>Entidad</returns>
        Task<T> SelectByKey(object ValueKey);
        
        /// <summary>
        /// Obtener todos los registros de la entidad
        /// </summary>
        /// <returns>Entidad</returns>
        Task<List<T>> SelectAll();

        /// <summary>
        /// Agregar un registro a la entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        Task AddItem(T entity);

        /// <summary>
        /// Elimina una entidad ingresada
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        Task RemoveItem(T entity);

        /// <summary>
        /// Actualiza los datos de una entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        Task UpdateItem(T entity);

        /// <summary>
        /// Busca un registro por medio de una expresión Lambda
        /// </summary>
        /// <param name="expr">Expresión  Lambda</param>
        /// <returns>Entidad</returns>
        Task<T> SelectByValues(Expression<Func<T, bool>> expr);
    }
}
