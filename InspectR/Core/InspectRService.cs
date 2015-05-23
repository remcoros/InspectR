namespace InspectR.Core
{
    using System;
    using System.Linq;

    using InspectR.Data;

    public class InspectRService
    {
        private readonly InspectRContext _dbContext;

        public InspectRService(InspectRContext dbContext)
        {
            _dbContext = dbContext;
        }

        public InspectorInfo CreateInspector(bool isprivate)
        {
            var inspector = new InspectorInfo
                                {
                                    IsPrivate = isprivate
                                };

            _dbContext.Inspectors.Add(inspector);
            _dbContext.SaveChanges();

            return inspector;
        }

        public void AddInspectorToUser(string userName, InspectorInfo info)
        {
            var user = _dbContext.GetUserProfile(userName);
            if (user == null)
            {
                user = CreateUserProfileInternal(userName);
            }

            if (user.Inspectors.All(i => i.Id != info.Id))
            {
                user.Inspectors.Add(info);
            }
            _dbContext.SaveChanges();
        }

        public void RemoveInspectorFromUser(string userName, Guid inspectorId)
        {
            var user = _dbContext.GetUserProfile(userName);
            if (user == null)
            {
                // todo: throw?
                return;
            }

            var found = user.Inspectors.FirstOrDefault(x => x.Id == inspectorId);
            if (found != null)
            {
                user.Inspectors.Remove(found);
            }

            _dbContext.SaveChanges();
        }

        public InspectRUserProfile CreateUserProfile(string userName)
        {
            var user = CreateUserProfileInternal(userName);
            _dbContext.SaveChanges();
            return user;
        }

        protected InspectRUserProfile CreateUserProfileInternal(string userName)
        {
            var user = new InspectRUserProfile(userName);
            _dbContext.UserProfiles.Add(user);
            return user;
        }

        public InspectorInfo GetInspectorInfoByKey(string id)
        {
            return _dbContext.GetInspectorInfoByKey(id);
        }
    }
}
