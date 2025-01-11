using backend.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Models.Validators
{
    public class BranchValidation : AbstractValidator<BranchModel>
    {
        public BranchValidation()
        {
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.BranchName).NotEmpty().WithMessage("Name is required");
        }
    }
}