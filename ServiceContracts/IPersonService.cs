using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonService
    {
        PersonResponse Add(PersonAddRequest person);
        PersonResponse Update(PersonUpdateRequest person);
        bool Delete(Guid id);
        List<PersonResponse> GetAll();
        PersonResponse GetById(Guid id);
        List<PersonResponse> GetFilteredPerson(string searchBy, string SearchString);
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOptions sortDir);
    }
}
