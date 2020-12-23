using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace YuLinTu.Practice.Authors
{
    public class AuthorManager : DomainService
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorManager(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<Author> CreateAsync(
            [NotNull] string firstName,
            [NotNull] string lastName,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));

            var existingAuthor = await authorRepository.FindByNameAsync(firstName, lastName);
            if (existingAuthor != null)
            {
                throw new AuthorAlreadyExistsException(firstName + lastName);
            }

            return new Author(
                GuidGenerator.Create(),
                firstName,
                lastName,
                birthDate,
                shortBio
            );
        }

        public async Task ChangeNameAsync(
            [NotNull] Author author,
            [NotNull] string newFirstName,
            [NotNull] string newLastName)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNullOrWhiteSpace(newFirstName, nameof(newFirstName));
            Check.NotNullOrWhiteSpace(newLastName, nameof(newLastName));

            var existingAuthor = await authorRepository.FindByNameAsync(newFirstName, newLastName);
            if (existingAuthor != null && existingAuthor.Id != author.Id)
            {
                throw new AuthorAlreadyExistsException(newFirstName + newLastName);
            }

            author.ChangeName(newFirstName, newLastName);
        }
    }
}