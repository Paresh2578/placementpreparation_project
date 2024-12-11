using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class BranchRepo : BranchInterface
    {
        private readonly ApplicationDbContext _context;
        public BranchRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async  Task<ResponseModel> AddBranch(BranchModel branch)
        {
            try{
                // Add branch to database
                await _context.Branches.AddAsync(branch);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    StatusCode= 201,
                    Message = "Branch Added Successfully"
                };
            }catch{
                return new ResponseModel
                {
                    StatusCode= 500,
                    Message = "Internal Server Error"
                };
            }
        }

       public async Task<ResponseModel> DeleteBranch(BranchModel branch)
        {
            try{ 
                // Remove branch from database
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    StatusCode= 200,
                    Message = "Branch Deleted Successfully"
                };
            }catch{
                return new ResponseModel
                {
                    StatusCode= 500,
                    Message = "Internal Server Error"
                };
            }
        }

       public async Task<ResponseModel> DeleteMultipleBranch(List<Guid> branchIds){
        try{
           // get all Branch to deleted
            var branchToDelete = await _context.Branches.Where(branch => branchIds.Contains(branch.BranchId)).ToListAsync();

            _context.Branches.RemoveRange(branchToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Branches"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }
      public async  Task<ResponseModel> GetAllBranches()
        {
            try{
                // Get all branches
                var branches = await _context.Branches.ToListAsync();
                 return new ResponseModel{StatusCode= 200, Data = branches , Message = "Branches fetched successfully"};
            }catch{
                return new ResponseModel
                    {
                        StatusCode= 500,
                        Message = "Internal Server Error"
                };
            }
        }

      public async  Task<ResponseModel> GetBranchById(Guid branchId)
        {
            try{
                // Get branch by id
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.BranchId == branchId);
                if(branch is null){
                    return new ResponseModel
                    {
                        StatusCode= 404,
                        Message = "Branch not found"
                    };
                }

                return new ResponseModel
                {
                    StatusCode= 200,
                    Message = "Branch fetched successfully",
                    Data = branch
                };
            }catch{
                return new ResponseModel
                {
                    StatusCode= 500,
                    Message = "Internal Server Error"
                };
            }
        }

       public async Task<ResponseModel> UpdateBranch(BranchModel newBranch)
        {
            try{
                // get branch by id
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.BranchId == newBranch.BranchId);
                if(branch is null){
                    return new ResponseModel
                    {
                        StatusCode= 404,
                        Message = "Branch not found"
                    };
                }

                // Update branch
                branch.BranchName = newBranch.BranchName;
            //    _context.Branches.Update(branch);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    StatusCode= 200,
                    Message = "Branch Updated Successfully"
                };
            }catch{
                return new ResponseModel
                {
                    StatusCode= 500,
                    Message = "Internal Server Error"
                };
        }
    }
    }
}
