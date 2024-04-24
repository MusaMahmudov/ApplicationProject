using AbilloLLCApplication.Business.DTOs.MessagesDTOs;
using AbilloLLCApplication.Business.Extensions;
using AbilloLLCApplication.Business.HelperServices.Interfaces;

using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Database.Contexts;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private static string ApplicationName = "AbilloLLCApp";
  

        public MessageService(IMapper mapper,IConfiguration configuration,IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

  
        public async Task<List<GetMessageDTO>> GetMessageAsync(string SecretFilePath,string SecretFilePathToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var cargoRepository = scope.ServiceProvider.GetService<ICargoRepository>();

                UserCredential credential;
                

                using (var stream = new FileStream(SecretFilePath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = SecretFilePathToken;
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List("me");

                request.MaxResults = 10;
                request.LabelIds = "INBOX";
                request.Q = $"from:(Example@mail.ru)";

                IList<Message> messages = request.Execute().Messages;







                DateTime dateTime = DateTime.UtcNow.AddMinutes(-1);
                long internalDateMs = (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;



                if (messages != null && messages.Count > 0)
                {
                    List<GetMessageDTO> messageDTOs = new List<GetMessageDTO>();
                    foreach (var message in messages)
                    {

                        var email = service.Users.Messages.Get("me", message.Id).Execute();


                        if (email.InternalDate > internalDateMs)
                        {


                            string base64String = email.Payload.Body.Data;
                            base64String = base64String.Replace('-', '+');
                            base64String = base64String.Replace('_', '/');

                            byte[] data = Convert.FromBase64String(base64String);
                            string decodedHtml = Encoding.UTF8.GetString(data);


                            HtmlDocument htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(decodedHtml);

                            string extractedText = htmlDoc.DocumentNode.InnerText.Trim();
                            int indexOfMiles = extractedText.IndexOf("MILES");
                            int indexOfPieces = extractedText.IndexOf("Pieces:");
                            int indexOfWeight = extractedText.IndexOf("Weight:");
                            int indexOfPickUpAt = extractedText.IndexOf("Pick-Up");
                            int indexOfDeliverTo = extractedText.IndexOf("Delivery");
                            int indexOfNotes = extractedText.IndexOf("Notes:");
                            int indexOfDims = extractedText.IndexOf("Dimensions:");


                            var messageDTO = _mapper.Map<GetMessageDTO>(email);
                            messageDTO.extractedText = extractedText;
                            messageDTO.CheckId = email.Id;

                            var toEmail = email.Payload.Headers.FirstOrDefault(h => h.Name == "Reply-To");
                            var fromEmail = email.Payload.Headers.FirstOrDefault(h => h.Name == "From");
                            messageDTO.ToEmail = toEmail?.Value;
                            messageDTO.FromEmail = toEmail?.Value;


                            if (indexOfNotes != -1)
                            {

                                messageDTO.HasNotes = true;


                                int indexOfLoadType = extractedText.IndexOf("Load Type");
                                if (indexOfLoadType != -1)
                                {
                                    messageDTO.Notes = extractedText.Substring(indexOfNotes + 6, indexOfLoadType - indexOfNotes - 6).Trim();
                                    if (string.IsNullOrEmpty(messageDTO.Notes))
                                    {
                                        messageDTO.Notes = "No notes";
                                    }
                                }


                            }



                            string DimsString = extractedText.Substring(indexOfDims + 11, 20);




                            int indexOfW = DimsString.IndexOf("W");
                            int indexOfL = DimsString.IndexOf("L");
                            int indexOfH = DimsString.IndexOf("H");
                            double width;
                            double length;
                            double height;
                            if (indexOfH != -1 && indexOfL != -1 && indexOfW != -1)
                            {
                                string heightString = DimsString.Substring(indexOfW, indexOfH - indexOfW).ExtractNumbers();
                                if (double.TryParse(heightString, out height))
                                {
                                    messageDTO.Height = height;
                                }
                                string lengthString = DimsString.Substring(0, indexOfL).ExtractNumbers();
                                if (double.TryParse(lengthString, out length))
                                {
                                    messageDTO.Length = length;
                                }
                                string widthString = DimsString.Substring(indexOfL, indexOfW - indexOfL).ExtractNumbers();
                                if (double.TryParse(widthString, out width))
                                {
                                    messageDTO.Width = width;
                                }
                            }




                            string weightStringDirty = extractedText.Substring(indexOfWeight + 6, 7).Trim();
                            string milesInStringDirty = string.Empty;
                            if (indexOfMiles != -1)
                            {
                                milesInStringDirty = extractedText.Substring(indexOfMiles - 10, 9).Trim();
                                milesInStringDirty = milesInStringDirty.ExtractNumbers();

                            }
                            string piecesInStringDirty = extractedText.Substring(indexOfPieces + 6, 9).Trim();
                            string pickUpAtStringDirty = extractedText.Substring(indexOfPickUpAt + 14, 40).Trim();
                            string deliverToStringDirty = extractedText.Substring(indexOfDeliverTo + 10, 30).Trim();
                            if (indexOfPickUpAt != -1)
                            {
                                string pickUpAt = extractedText.Substring(indexOfPickUpAt, 40);
                                int index = pickUpAt.IndexOf(",");
                                if (index != -1 && index > 7)
                                {
                                    messageDTO.PickUpCity = pickUpAt.Substring(7, index - 7);

                                }
                                else
                                {
                                    messageDTO.PickUpCity = "No Info";
                                }
                            }
                            if (indexOfDeliverTo != -1)
                            {
                                string deliverToString = extractedText.Substring(indexOfDeliverTo, 40);
                                int index = deliverToString.IndexOf(",");
                                if (index != -1 && index > 8)
                                {
                                    messageDTO.DeliverToCity = deliverToString.Substring(8, index - 8);

                                }
                                else
                                {
                                    messageDTO.DeliverToCity = "No Info";
                                }

                            }


                            piecesInStringDirty = piecesInStringDirty.ExtractNumbers();
                            weightStringDirty = weightStringDirty.ExtractNumbers();

                            pickUpAtStringDirty = pickUpAtStringDirty.ExtractNumbers();
                            deliverToStringDirty = deliverToStringDirty.ExtractNumbers();

                            if (pickUpAtStringDirty.Length > 5)
                            {
                                pickUpAtStringDirty = pickUpAtStringDirty.Substring(0, 5);
                            }
                            if (deliverToStringDirty.Length > 5)
                            {
                                deliverToStringDirty = deliverToStringDirty.Substring(0, 5);
                            }

                            messageDTO.DeliverToZipcode = deliverToStringDirty;
                            messageDTO.PickUpZipcode = pickUpAtStringDirty;


                            int miles;
                            int pieces;
                            int weight;


                            if (int.TryParse(milesInStringDirty, out miles))
                            {
                                messageDTO.Miles = miles;
                            }
                            else
                            {
                                messageDTO.hasMiles = false;

                            }
                            if (int.TryParse(piecesInStringDirty, out pieces))
                            {
                                messageDTO.Pieces = pieces;
                            }
                            else
                            { messageDTO.hasPieces = false; }
                            if (int.TryParse(weightStringDirty, out weight))
                            {
                                messageDTO.Weight = weight;
                            }
                            else
                            {
                                messageDTO.HasWeight = false;
                            }


                            messageDTOs.Add(messageDTO);

                        }

                    }

                    return messageDTOs;
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
