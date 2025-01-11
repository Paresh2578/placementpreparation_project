using backend.Models;

namespace backend.data.Interface
{
    public interface BranchInterface
    {
        Task<ResponseModel> GetAllBranches();
        Task<ResponseModel> GetBranchById(Guid branchId);
        Task<ResponseModel> AddBranch(BranchModel branch);
        Task<ResponseModel> UpdateBranch(BranchModel newBranch);
        Task<ResponseModel> DeleteBranch(BranchModel branch);
        Task<ResponseModel> DeleteMultipleBranch(List<Guid> branchIds);
    }
}
