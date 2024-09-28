using AutoMapper;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _IUserRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            _IUserRepository = articleRepository;
            _mapper = mapper;
        }
    }
}
