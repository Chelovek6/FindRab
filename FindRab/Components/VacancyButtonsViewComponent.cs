using Microsoft.AspNetCore.Mvc;

namespace FindRab.Components
{
    public class VacancyButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentUserId, int ownerId, int vacancyId)
        {
            var isOwner = currentUserId == ownerId;
            return View(isOwner ? "OwnerButtons" : "UserButtons", vacancyId);
        }
    }
}
