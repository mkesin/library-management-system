﻿using System.Linq;
using Library.Models.Branch;
using LibraryData;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BranchController : Controller
    {
        private readonly ILibraryBranchService _branchService;

        // create a constructor takes branchservice
        public BranchController(ILibraryBranchService branchService)
        {
            // save branchService param off into a private field 
            // to have access in the rest of the controller
            _branchService = branchService;
        }

        public IActionResult Index()
        {
            var branchModels = _branchService.GetAll()
                .Select(br => new BranchDetailModel
                {
                    Id = br.Id,
                    BranchName = br.Name,
                    NumberOfAssets = _branchService.GetAssetCount(br.LibraryAssets),
                    NumberOfPatrons = _branchService.GetPatronCount(br.Patrons),
                    IsOpen = _branchService.IsBranchOpen(br.Id)
                }).ToList();

            var model = new BranchIndexModel
            {
                Branches = branchModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branchService.Get(id);
            var model = new BranchDetailModel
            {
                BranchName = branch.Name,
                Description = branch.Description,
                Address = branch.Address,
                Telephone = branch.Telephone,
                BranchOpenedDate = branch.OpenDate.ToString("yyyy-MM-dd"),
                NumberOfPatrons = _branchService.GetPatronCount(branch.Patrons),
                NumberOfAssets = _branchService.GetAssetCount(branch.LibraryAssets),
                TotalAssetValue = _branchService.GetAssetsValue(id),
                ImageUrl = branch.ImageUrl,
                HoursOpen = _branchService.GetBranchHours(id)
            };

            return View(model);
        }
    }
}