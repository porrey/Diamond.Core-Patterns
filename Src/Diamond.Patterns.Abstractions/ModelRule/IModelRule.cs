using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
    public interface IModelRule<TEntity>
    {
        /// <summary>
        /// Validate model based on the defined rule asynchronously
        /// </summary>
        /// <param name="model">TEntity to be validated</param>
        /// <returns>bool and consolidated error message</returns>
        Task<(bool, string)> ValidateModelAsync(TEntity model);
        /// <summary>
        /// Group name to distinguish between different rules
        /// </summary>
        string Group { get; set; }
    }
}
