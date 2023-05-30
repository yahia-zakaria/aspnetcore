using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse> Add(PersonAddRequest person);
        Task<PersonResponse> Update(PersonUpdateRequest person);
        Task<bool> Delete(Guid id);
        Task<List<PersonResponse>> GetAll();
        Task<PersonResponse> GetById(Guid id);
		Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string SearchString);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOptions sortDir);
        Task<MemoryStream> GetPersonsCSV();
        Task<MemoryStream> GetPersonsExcel();

	}
}
