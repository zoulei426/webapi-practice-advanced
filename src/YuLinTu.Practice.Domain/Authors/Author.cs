using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace YuLinTu.Practice.Authors
{
    public class Author : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }

        #endregion Properties

        #region Ctor

        private Author()
        {
        }

        internal Author(
            Guid id,
            [NotNull] string firstName,
            [NotNull] string lastName,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
            : base(id)
        {
            SetName(firstName, lastName);
            BirthDate = birthDate;
            ShortBio = shortBio;
        }

        #endregion Ctor

        #region Methods

        internal Author ChangeName([NotNull] string firstName, [NotNull] string lastName)
        {
            SetName(firstName, lastName);
            return this;
        }

        private void SetName([NotNull] string firstName, [NotNull] string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(
               firstName,
               nameof(firstName),
               maxLength: AuthorConsts.MaxNameLength
           );

            LastName = Check.NotNullOrWhiteSpace(
               lastName,
               nameof(lastName),
               maxLength: AuthorConsts.MaxNameLength
           );
        }

        #endregion Methods
    }
}