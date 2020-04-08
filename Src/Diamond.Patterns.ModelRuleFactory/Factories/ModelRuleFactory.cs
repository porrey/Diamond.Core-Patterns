using Diamond.Patterns.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Patterns.ModelRuleFactory
{
    public class ModelRuleFactory : IModelRuleFactory
    {
        public ModelRuleFactory(IObjectFactory objectFactory)
        {
            this.ObjectFactory = objectFactory;
        }

        protected IObjectFactory ObjectFactory { get; set; }

        public Task<IEnumerable<IModelRule<TInterface>>> GetAllAsync<TInterface>()
        {
            return Task.FromResult(this.ObjectFactory.GetAllInstances<IModelRule<TInterface>>());
        }

        public Task<IEnumerable<IModelRule<TInterface>>> GetAllAsync<TInterface>(string group)
        {
            var instances = this.ObjectFactory.GetAllInstances<IModelRule<TInterface>>();
            return Task.FromResult(instances.Where(x => x.Group == group));
        }
    }
}
