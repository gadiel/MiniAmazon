using System.Collections.Generic;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;

namespace MiniAmazon.Web.Infrastructure
{
    public class CategoryGeneralModel
    {
        public CategoryGeneralModel(IEnumerable<object> toDefine, IRepository _repository)
        {
            Define(toDefine,_repository);
        }


        public void Define(IEnumerable<object> toDefine, IRepository _repository)
        {
            _Object = toDefine;
            GetCategoryData(_repository);
        }

        private void GetCategoryData(IRepository _repository)
        {
            Categories = _repository.Query<Category>(x => x == x);
        }

        public IEnumerable<object> _Object { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }

    }
}