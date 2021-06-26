﻿using MarvelAPI.Parameters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelAPI
{
    public class StoryRequest : BaseRequest
    {
        public StoryRequest(string publicApiKey, string privateApiKey, IRestClient client, bool? useGZip = null) 
            : base(publicApiKey, privateApiKey, client, useGZip)
        {
        }

        /// <summary>
        /// Fetches lists of comic stories with optional filters.
        /// </summary>
        /// <returns>
        /// Lists of comic stories
        /// </returns>
        public IEnumerable<Story> GetStories(GetStories model)
        {
            var request = CreateRequest("/stories");

            if (model.ModifiedSince.HasValue)
            {
                request.AddParameter("modifiedSince", model.ModifiedSince.Value.ToString("yyyy-MM-dd"));
            }

            request.AddParameterList(model.Comics, "comics");
            request.AddParameterList(model.Series, "series");
            request.AddParameterList(model.Events, "events");
            request.AddParameterList(model.Creators, "creators");
            request.AddParameterList(model.Characters, "characters");

            var availableOrderBy = new List<OrderBy>
            {
                OrderBy.Id,
                OrderBy.IdDesc,
                OrderBy.Modified,
                OrderBy.ModifiedDesc
            };
            request.AddOrderByParameterList(model.Order, availableOrderBy);

            if (model.Limit.HasValue && model.Limit.Value > 0)
            {
                request.AddParameter("limit", model.Limit.Value.ToString());
            }
            if (model.Offset.HasValue && model.Offset.Value > 0)
            {
                request.AddParameter("offset", model.Offset.Value.ToString());
            }

            IRestResponse<Wrapper<Story>> response = Client.Execute<Wrapper<Story>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results;
        }

        /// <summary>
        /// This method fetches a single comic story resource. It is the canonical URI for any comic story resource provided by the API.
        /// </summary>
        /// <returns>
        /// A single comic story resource
        /// </returns>
        public Story GetStory(int StoryId)
        {
            var request = CreateRequest($"/stories/{StoryId}");

            IRestResponse<Wrapper<Story>> response = Client.Execute<Wrapper<Story>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results.FirstOrDefault(story => story.Id == StoryId);
        }

        /// <summary>
        /// Fetches lists of comic characters appearing in a single story, with optional filters.
        /// </summary>
        /// <returns>
        /// Lists of comic characters appearing in a single story
        /// </returns>
        public IEnumerable<Character> GetCharactersForStory(GetCharactersForStory model)
        {
            var request = CreateRequest($"/stories/{model.StoryId}/characters");

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                request.AddParameter("name", model.Name);
            }
            if (!string.IsNullOrWhiteSpace(model.NameStartsWith))
            {
                request.AddParameter("nameStartsWith", model.NameStartsWith);
            }
            if (model.ModifiedSince.HasValue)
            {
                request.AddParameter("modifiedSince", model.ModifiedSince.Value.ToString("yyyy-MM-dd"));
            }

            request.AddParameterList(model.Comics, "comics");
            request.AddParameterList(model.Events, "events");
            request.AddParameterList(model.Series, "series");

            var availableOrderBy = new List<OrderBy>
            {
                OrderBy.Name,
                OrderBy.NameDesc,
                OrderBy.Modified,
                OrderBy.ModifiedDesc
            };
            request.AddOrderByParameterList(model.Order, availableOrderBy);

            if (model.Limit.HasValue && model.Limit.Value > 0)
            {
                request.AddParameter("limit", model.Limit.Value.ToString());
            }
            if (model.Offset.HasValue && model.Offset.Value > 0)
            {
                request.AddParameter("offset", model.Offset.Value.ToString());
            }

            IRestResponse<Wrapper<Character>> response = Client.Execute<Wrapper<Character>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results;
        }

        /// <summary>
        /// Fetches lists of comics in which a specific story appears, with optional filters.
        /// </summary>
        /// <returns>
        /// Lists of comics in which a specific story appears
        /// </returns>
        public IEnumerable<Comic> GetComicsForStory(GetComicsForStory model)
        {
            var request = CreateRequest($"/stories/{model.StoryId}/comics");

            if (model.Format.HasValue)
            {
                request.AddParameter("format", model.Format.Value.ToParameter());
            }
            if (model.FormatType.HasValue)
            {
                request.AddParameter("formatType", model.FormatType.Value.ToParameter());
            }
            if (model.NoVariants.HasValue)
            {
                request.AddParameter("noVariants", model.NoVariants.Value.ToString().ToLower());
            }
            if (model.DateDescript.HasValue)
            {
                request.AddParameter("dateDescriptor", model.DateDescript.Value.ToParameter());
            }
            if (model.DateRangeBegin.HasValue && model.DateRangeEnd.HasValue)
            {
                if (model.DateRangeBegin.Value <= model.DateRangeEnd.Value)
                {
                    request.AddParameter("dateRange", $"{model.DateRangeBegin.Value.ToString("yyyy-MM-dd")},{model.DateRangeEnd.Value.ToString("yyyy-MM-dd")}");
                }
                else
                {
                    throw new ArgumentException("DateRangeBegin must be greater than DateRangeEnd");
                }
            }
            else if (model.DateRangeBegin.HasValue || model.DateRangeEnd.HasValue)
            {
                throw new ArgumentException("Date Range requires both a start and end date");
            }
            if (model.HasDigitalIssue.HasValue)
            {
                request.AddParameter("hasDigitalIssue", model.HasDigitalIssue.Value.ToString().ToLower());
            }
            if (model.ModifiedSince.HasValue)
            {
                request.AddParameter("modifiedSince", model.ModifiedSince.Value.ToString("yyyy-MM-dd"));
            }

            request.AddParameterList(model.Creators, "creators");
            request.AddParameterList(model.Characters, "characters");
            request.AddParameterList(model.Series, "series");
            request.AddParameterList(model.Events, "events");
            request.AddParameterList(model.SharedAppearances, "sharedAppearances");
            request.AddParameterList(model.Collaborators, "collaborators");

            var availableOrderBy = new List<OrderBy>
            {
                OrderBy.FocDate,
                OrderBy.FocDateDesc,
                OrderBy.OnSaleDate,
                OrderBy.OnSaleDateDesc,
                OrderBy.Title,
                OrderBy.TitleDesc,
                OrderBy.IssueNumber,
                OrderBy.IssueNumberDesc,
                OrderBy.Modified,
                OrderBy.ModifiedDesc
            };
            request.AddOrderByParameterList(model.Order, availableOrderBy);

            if (model.Limit.HasValue && model.Limit.Value > 0)
            {
                request.AddParameter("limit", model.Limit.Value.ToString());
            }
            if (model.Offset.HasValue && model.Offset.Value > 0)
            {
                request.AddParameter("offset", model.Offset.Value.ToString());
            }

            IRestResponse<Wrapper<Comic>> response = Client.Execute<Wrapper<Comic>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results;
        }

        /// <summary>
        /// Fetches lists of comic creators whose work appears in a specific story, with optional filters.
        /// </summary>
        /// <returns>
        /// Lists of comic creators whose work appears in a specific story
        /// </returns>
        public IEnumerable<Creator> GetCreatorsForStory(GetCreatorsForStory model)
        {
            var request = CreateRequest($"/stories/{model.StoryId}/creators");

            if (!string.IsNullOrWhiteSpace(model.FirstName))
            {
                request.AddParameter("firstName", model.FirstName);
            }
            if (!string.IsNullOrWhiteSpace(model.MiddleName))
            {
                request.AddParameter("middleName", model.MiddleName);
            }
            if (!string.IsNullOrWhiteSpace(model.LastName))
            {
                request.AddParameter("lastName", model.LastName);
            }
            if (!string.IsNullOrWhiteSpace(model.Suffix))
            {
                request.AddParameter("suffix", model.Suffix);
            }
            if (!string.IsNullOrWhiteSpace(model.NameStartsWith))
            {
                request.AddParameter("nameStartsWith", model.NameStartsWith);
            }
            if (!string.IsNullOrWhiteSpace(model.FirstNameStartsWith))
            {
                request.AddParameter("firstNameStartsWith", model.FirstNameStartsWith);
            }
            if (!string.IsNullOrWhiteSpace(model.MiddleNameStartsWith))
            {
                request.AddParameter("middleNameStartsWith", model.MiddleNameStartsWith);
            }
            if (!string.IsNullOrWhiteSpace(model.LastNameStartsWith))
            {
                request.AddParameter("lastNameStartsWith", model.LastNameStartsWith);
            }
            if (model.ModifiedSince.HasValue)
            {
                request.AddParameter("modifiedSince", model.ModifiedSince.Value.ToString("yyyy-MM-dd"));
            }

            request.AddParameterList(model.Comics, "comics");
            request.AddParameterList(model.Series, "series");
            request.AddParameterList(model.Events, "events");

            var availableOrderBy = new List<OrderBy>
            {
                OrderBy.FirstName,
                OrderBy.FirstNameDesc,
                OrderBy.MiddleName,
                OrderBy.MiddleNameDesc,
                OrderBy.LastName,
                OrderBy.LastNameDesc,
                OrderBy.Suffix,
                OrderBy.SuffixDesc,
                OrderBy.Modified,
                OrderBy.ModifiedDesc
            };
            request.AddOrderByParameterList(model.Order, availableOrderBy);

            if (model.Limit.HasValue && model.Limit.Value > 0)
            {
                request.AddParameter("limit", model.Limit.Value.ToString());
            }
            if (model.Offset.HasValue && model.Offset.Value > 0)
            {
                request.AddParameter("offset", model.Offset.Value.ToString());
            }

            IRestResponse<Wrapper<Creator>> response = Client.Execute<Wrapper<Creator>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results;
        }

        /// <summary>
        /// Fetches lists of events in which a specific story appears, with optional filters.
        /// </summary>
        /// <returns>
        /// Lists of events in which a specific story appears
        /// </returns>
        public IEnumerable<Event> GetEventsForStory(GetEventsForStory model)
        {
            var request = CreateRequest($"/stories/{model.StoryId}/events");

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                request.AddParameter("name", model.Name);
            }
            if (!string.IsNullOrWhiteSpace(model.NameStartsWith))
            {
                request.AddParameter("nameStartsWith", model.NameStartsWith);
            }
            if (model.ModifiedSince.HasValue)
            {
                request.AddParameter("modifiedSince", model.ModifiedSince.Value.ToString("yyyy-MM-dd"));
            }

            request.AddParameterList(model.Creators, "creators");
            request.AddParameterList(model.Characters, "characters");
            request.AddParameterList(model.Series, "series");
            request.AddParameterList(model.Comics, "comics");

            var availableOrderBy = new List<OrderBy>
            {
                OrderBy.Name,
                OrderBy.NameDesc,
                OrderBy.StartDate,
                OrderBy.StartDateDesc,
                OrderBy.Modified,
                OrderBy.ModifiedDesc
            };
            request.AddOrderByParameterList(model.Order, availableOrderBy);

            if (model.Limit.HasValue && model.Limit.Value > 0)
            {
                request.AddParameter("limit", model.Limit.Value.ToString());
            }
            if (model.Offset.HasValue && model.Offset.Value > 0)
            {
                request.AddParameter("offset", model.Offset.Value.ToString());
            }

            IRestResponse<Wrapper<Event>> response = Client.Execute<Wrapper<Event>>(request);

            HandleResponseErrors(response);

            return response.Data.Data.Results;
        }
    }
}
