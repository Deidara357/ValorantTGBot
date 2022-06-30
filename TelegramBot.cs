using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ValorantTGBot
{
    public class TelegramBot
    {
        private TelegramBotClient _client;
        private CancellationToken _cancellationTokenSource;
        private ReceiverOptions _receiverOptions;
        private ApiMethods _apiMethods;
        private string _agentName;
        private bool wantToAddAgent;
        private bool wantToDeleteAgent;
        private bool wantToAddMap;
        private bool wantToDeleteMap;

        public TelegramBot(string token)
        {
            _client = new TelegramBotClient(token);
            _cancellationTokenSource = new CancellationToken();
            _receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

            _apiMethods = new ApiMethods();
        }

        public async Task Start()
        {
            _client.StartReceiving(HandlerUpdate, HandleError, _receiverOptions, _cancellationTokenSource);
            var me = await _client.GetMeAsync();
            Console.WriteLine($"Бот {me.Username} почав працювати");
            Console.ReadKey();
        }

        private Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);

            return Task.CompletedTask;
        }

        private async Task HandlerUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }

            await ReplyMessage(botClient, update.Message);
        }

        private async Task ReplyMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Привiт {message.From.FirstName}!" +
                    " Цей бот показує інформацію про" +
                    " вiдому гру Valorant\nВиберiть команду /keyboard, щоб відкрити меню");

                return;
            }
            else if (message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = MenuButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть пункт меню",
                    replyMarkup: replyKeyboardMarkup);

                return;
            }
            else if (message.Text == "Agents")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = AgentsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть героя\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                wantToAddAgent = false;
                wantToDeleteAgent = false;

                return;
            }
            else if (IsAgentsButtonPressed(message) && wantToAddAgent == false && wantToDeleteAgent == false)
            {
                var result = _apiMethods.AgentInfo(message.Text).Result;

                _agentName = message.Text;

                string agentInfo = $"Agent Name: {result.AgentName}\n" +
                    $"Description: {result.Description}\n" +
                    $"Role: {result.Role}\n" +
                    $"Role Description: {result.RoleDescription}\n";

                await botClient.SendPhotoAsync(message.Chat.Id, photo: result.FullPortrait,
                    caption: $"{agentInfo}\nМеню /keyboard");

                await ShowAbilities(botClient, message);

                return;
            }
            else if (message.Text == "Passive")
            {
                var result = _apiMethods.AgentInfo(_agentName).Result;

                if (result.AgentsAbilities.Count == 5)
                {
                    string agentAbilities;

                    agentAbilities = $"{result.AgentsAbilities[4].Slot}\n" +
                            $"Ability Name: {result.AgentsAbilities[4].DisplayName}\n" +
                            $"Description: {result.AgentsAbilities[4].Description}";

                    await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");
                }
                else
                {
                    string agentAbilities = "В цього героя немає такої здiбностi";

                    await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");
                }

                return;
            }
            else if (message.Text == "Ability 1")
            {
                var result = _apiMethods.AgentInfo(_agentName).Result;

                string agentAbilities;

                agentAbilities = $"{result.AgentsAbilities[0].Slot}\n" +
                        $"Ability Name: {result.AgentsAbilities[0].DisplayName}\n" +
                        $"Description: {result.AgentsAbilities[0].Description}";

                await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");

                return;
            }
            else if (message.Text == "Ability 2")
            {
                var result = _apiMethods.AgentInfo(_agentName).Result;

                string agentAbilities;

                agentAbilities = $"{result.AgentsAbilities[1].Slot}\n" +
                        $"Ability Name: {result.AgentsAbilities[1].DisplayName}\n" +
                        $"Description: {result.AgentsAbilities[1].Description}";

                await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");

                return;
            }
            else if (message.Text == "Ability 3")
            {
                var result = _apiMethods.AgentInfo(_agentName).Result;

                string agentAbilities;

                agentAbilities = $"{result.AgentsAbilities[2].Slot}\n" +
                        $"Ability Name: {result.AgentsAbilities[2].DisplayName}\n" +
                        $"Description: {result.AgentsAbilities[2].Description}";

                await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");

                return;
            }
            else if (message.Text == "Ultimate")
            {
                var result = _apiMethods.AgentInfo(_agentName).Result;

                string agentAbilities;

                agentAbilities = $"{result.AgentsAbilities[3].Slot}\n" +
                        $"Ability Name: {result.AgentsAbilities[3].DisplayName}\n" +
                        $"Description: {result.AgentsAbilities[3].Description}";

                await botClient.SendTextMessageAsync(message.Chat.Id, agentAbilities);
                await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");

                return;
            }
            else if (message.Text == "Maps")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = MapsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть карту\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                wantToAddMap = false;
                wantToDeleteMap = false;

                return;
            }
            else if (IsMapsButtonPressed(message) && wantToAddMap == false && wantToDeleteMap == false)
            {
                var result = _apiMethods.MapInfo(message.Text).Result;

                string mapInfo = $"Map Name: {result.MapName}";

                await botClient.SendPhotoAsync(message.Chat.Id, photo: result.Photo,
                    caption: $"{mapInfo}\n\nМеню /keyboard");

                return;
            }
            else if (message.Text == "Weapons")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = WeaponsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть зброю\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                return;
            }
            else if (IsWeaponsButtonPressed(message))
            {
                var result = _apiMethods.WeaponInfo(message.Text).Result;

                string weaponInfo, damageRanges;

                if (message.Text == "Холодное оружие")
                {
                    weaponInfo = $"Weapon Name: {result.WeaponName}";

                    await botClient.SendPhotoAsync(message.Chat.Id, photo: result.Photo, caption: weaponInfo);
                }
                else
                {
                    weaponInfo = $"Weapon Name: {result.WeaponName}\n" +
                        $"Fire Rate: {result.FireRate}\n" +
                        $"Magazine Size: {result.MagazineSize}\n" +
                        $"Equip Time In Seconds: {result.EquipTimeSeconds}\n" +
                        $"Reload Time In Seconds: {result.ReloadTimeSeconds}\n" +
                        $"Cost: {result.Cost}\n" +
                        $"Weapon Type: {result.WeaponType}\n";

                    await botClient.SendPhotoAsync(message.Chat.Id, photo: result.Photo, caption: weaponInfo);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Damage Ranges:");

                    for (int i = 0; i < result.Damage.Count; i++)
                    {
                        damageRanges = $"Range Start Meters: {result.Damage[i].RangeStartMeters}\n" +
                            $"Range End Meters: {result.Damage[i].RangeEndMeters}\n" +
                            $"Head Damage: {result.Damage[i].HeadDamage}\n" +
                            $"Body Damage: {result.Damage[i].BodyDamage}\n" +
                            $"Leg Gamage: {result.Damage[i].LegDamage}\n";

                        await botClient.SendTextMessageAsync(message.Chat.Id, damageRanges);
                    }
                }

                await botClient.SendTextMessageAsync(message.Chat.Id, "Меню /keyboard");

                return;
            }
            else if (message.Text == "Skins")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = SkinsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть зброю, щоб переглянути скіни\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                return;
            }
            else if (IsSkinsButtonPressed(message))
            {
                var result = _apiMethods.WeaponSkins(message.Text.Remove(message.Text.Length - 1)).Result;

                string weaponSkins;
                weaponSkins = $"Weapon Name: {result.WeaponName}\n";

                await botClient.SendTextMessageAsync(message.Chat.Id, weaponSkins);

                for (int i = 0; i < result.WeaponSkins.Count; i++)
                {
                    weaponSkins = $"Skin Name: {result.WeaponSkins[i].DisplayName}";

                    if (result.WeaponSkins[i].DisplayName == "Якорь \"Нептун\""
                        || result.WeaponSkins[i].DisplayName == "Нож \"Люкс\""
                        || result.WeaponSkins[i].DisplayName == "Marshal \"Монарх\""
                        || result.WeaponSkins[i].DisplayName == "Frenzy \"Коалиция: Кобра\""
                        || result.WeaponSkins[i].DisplayName == "Bulldog \"Блеск\""
                        || result.WeaponSkins[i].DisplayName == "Guardian \"Прайм\""
                        || result.WeaponSkins[i].DisplayName == "Guardian \"Монарх\""
                        || result.WeaponSkins[i].DisplayName == "Judge \"Коалиция: Кобра\""
                        || result.WeaponSkins[i].DisplayName == "Judge \"Сенсация\""
                        || result.WeaponSkins[i].DisplayName == "Холодное оружие"
                        || result.WeaponSkins[i].DisplayName == (message.Text.Remove(message.Text.Length - 1) + " \"Стандарт\""))
                    {
                        continue;
                    }
                    if (result.WeaponSkins[i].DisplayIcon != null)
                    {
                        await botClient.SendPhotoAsync(message.Chat.Id, photo: result.WeaponSkins[i].DisplayIcon,
                            caption: weaponSkins);
                    }
                    else
                    {
                        continue;
                    }
                }

                await botClient.SendTextMessageAsync(message.Chat.Id, "Меню /keyboard");

                return;
            }
            else if (message.Text == "Favourite Agents")
            {
                var buttons = FavAgentsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть дiю, яку хочете виконати\nМеню /keyboard",
                    replyMarkup: buttons);

                return;
            }
            else if (message.Text == "Show Agents")
            {
                var result = _apiMethods.ShowFavouriteAgents().Result;

                if (result.Count == 0)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ви нiкого ще не добавляли");
                }
                else
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        string agentInfo = $"Agent Name: {result[i].AgentName}\n" +
                        $"Description: {result[i].Description}\n" +
                        $"Role: {result[i].Role}\n";

                        await botClient.SendPhotoAsync(message.Chat.Id, photo: result[i].FullPortrait, caption: agentInfo);
                    }
                }

                await botClient.SendTextMessageAsync(message.Chat.Id, "Меню /keyboard");

                return;
            }
            else if (message.Text == "Add Agent")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = AgentsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть героя, якого хочете добавити\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                wantToAddAgent = true;

                return;
            }
            else if (IsAgentsButtonPressed(message) && wantToAddAgent == true)
            {
                wantToAddAgent = false;

                var result = _apiMethods.AddFavouriteAgent(message.Text).Result;

                var buttons = FavAgentsButtons();

                if (result == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Цей герой вже є в списку!\nМеню /keyboard",
                        replyMarkup: buttons);
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ви успiшно добавили героя в список!\nМеню /keyboard",
                        replyMarkup: buttons);
                }

                return;
            }
            else if (message.Text == "Delete Agent")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = AgentsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть героя, якого хочете видалити\nМеню /keyboard",
                    replyMarkup: replyKeyboardMarkup);

                wantToDeleteAgent = true;

                return;
            }
            else if (IsAgentsButtonPressed(message) && wantToDeleteAgent == true)
            {
                wantToDeleteAgent = false;

                var result = _apiMethods.DeleteFavouriteAgent(message.Text).Result;

                var buttons = FavAgentsButtons();

                if (result == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Цього героя немає в списку!\nМеню /keyboard",
                        replyMarkup: buttons);
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ви успiшно видалили героя із списку!\nМеню /keyboard",
                        replyMarkup: buttons);
                }

                return;
            }
            else if (message.Text == "Favourite Maps")
            {
                var buttons = FavMapsButtons();

                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть дiю, яку хочете виконати\nМеню /keyboard",
                    replyMarkup: buttons);

                return;
            }
            //else if (message.Text == "Show Maps")
            //{
            //    var result = _apiMethods.ShowFavouriteMaps().Result;

            //    if (result.Count == 0)
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Ви ще не добавляли карт");
            //    }
            //    else
            //    {
            //        for (int i = 0; i < result.Count; i++)
            //        {
            //            string agentInfo = $"Map Name: {result[i].MapName}\n";

            //            await botClient.SendPhotoAsync(message.Chat.Id, photo: result[i].Photo, caption: agentInfo);
            //        }
            //    }

            //    await botClient.SendTextMessageAsync(message.Chat.Id, "Меню /keyboard");

            //    return;
            //}
            //else if (message.Text == "Add Map")
            //{
            //    ReplyKeyboardMarkup replyKeyboardMarkup = MapsButtons();

            //    await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть карту, яку хочете добавити\nМеню /keyboard",
            //        replyMarkup: replyKeyboardMarkup);

            //    wantToAddMap = true;

            //    return;
            //}
            //else if (IsMapsButtonPressed(message) && wantToAddMap == true)
            //{
            //    wantToAddMap = false;

            //    var result = _apiMethods.AddFavouriteMap(message.Text).Result;

            //    var buttons = FavMapsButtons();

            //    if (result == null)
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Ця карта вже є в списку!\nМеню /keyboard",
            //            replyMarkup: buttons);
            //    }
            //    else
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Ви успiшно добавили карту в список!\nМеню /keyboard",
            //            replyMarkup: buttons);
            //    }

            //    return;
            //}
            //else if (message.Text == "Delete Map")
            //{
            //    ReplyKeyboardMarkup replyKeyboardMarkup = MapsButtons();

            //    await botClient.SendTextMessageAsync(message.Chat.Id, "Виберiть карту, яку хочете видалити\nМеню /keyboard",
            //        replyMarkup: replyKeyboardMarkup);

            //    wantToDeleteMap = true;

            //    return;
            //}
            //else if (IsMapsButtonPressed(message) && wantToDeleteMap == true)
            //{
            //    wantToDeleteMap = false;

            //    var result = _apiMethods.DeleteFavouriteMap(message.Text).Result;

            //    var buttons = FavMapsButtons();

            //    if (result == null)
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Цiєї карти немає в списку!\nМеню /keyboard",
            //            replyMarkup: buttons);
            //    }
            //    else
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Ви успiшно видалили карту із списку!\nМеню /keyboard",
            //            replyMarkup: buttons);
            //    }

            //    return;
            //}
            else if (message.Text == "News")
            {
                var result = _apiMethods.GetNews().Result;

                if (result == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Новин немає!");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, result);
                }

                return;
            }
            else if (message.Text == "Tournaments")
            {
                var result = _apiMethods.GetTournaments().Result;

                if (result == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Турнiрiв немає!");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, result);
                }

                return;
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ви ввели невідому команду!\nМеню /keyboard");

                return;
            }
        }

        private bool IsSkinsButtonPressed(Message message)
        {
            if (message.Text == "ClassicS" || message.Text == "ShortyS" || message.Text == "FrenzyS"
                || message.Text == "GhostS" || message.Text == "SheriffS" || message.Text == "StingerS"
                || message.Text == "BuckyS" || message.Text == "JudgeS" || message.Text == "BulldogS"
                || message.Text == "GuardianS" || message.Text == "PhantomS" || message.Text == "VandalS"
                || message.Text == "MarshalS" || message.Text == "OperatorS" || message.Text == "AresS"
                || message.Text == "OdinS" || message.Text == "Холодное оружиеS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsWeaponsButtonPressed(Message message)
        {
            if (message.Text == "Classic" || message.Text == "Shorty" || message.Text == "Frenzy"
                || message.Text == "Ghost" || message.Text == "Sheriff" || message.Text == "Stinger"
                || message.Text == "Spectre" || message.Text == "Bucky" || message.Text == "Judge"
                || message.Text == "Bulldog" || message.Text == "Guardian" || message.Text == "Phantom"
                || message.Text == "Vandal" || message.Text == "Marshal" || message.Text == "Operator"
                || message.Text == "Ares" || message.Text == "Odin" || message.Text == "Холодное оружие")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsMapsButtonPressed(Message message)
        {
            if (message.Text == "Split" || message.Text == "Haven" || message.Text == "Ascent"
                || message.Text == "Breeze" || message.Text == "Bind" || message.Text == "Icebox"
                || message.Text == "Fracture" || message.Text == "Pearl" || message.Text == "Стрельбище")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAgentsButtonPressed(Message message)
        {
            if (message.Text == "Astra" || message.Text == "Breach" || message.Text == "Brimstone"
                || message.Text == "Chamber" || message.Text == "Cypher" || message.Text == "Fade"
                || message.Text == "Jett" || message.Text == "KAY/O" || message.Text == "Killjoy"
                || message.Text == "Neon" || message.Text == "Omen" || message.Text == "Phoenix"
                || message.Text == "Raze" || message.Text == "Reyna" || message.Text == "Sage"
                || message.Text == "Skye" || message.Text == "Sova" || message.Text == "Viper"
                || message.Text == "Yoru")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ReplyKeyboardMarkup SkinsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "ClassicS", "ShortyS", "FrenzyS", "GhostS" },
                            new KeyboardButton[] { "SheriffS", "StingerS", "BuckyS", "JudgeS" },
                            new KeyboardButton[] { "BulldogS", "GuardianS", "PhantomS", "VandalS" },
                            new KeyboardButton[] { "MarshalS", "OperatorS", "AresS", "OdinS" },
                            new KeyboardButton[] { "Холодное оружиеS" }
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup WeaponsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Classic", "Shorty", "Frenzy", "Ghost" },
                            new KeyboardButton[] { "Sheriff", "Stinger", "Spectre", "Bucky" },
                            new KeyboardButton[] { "Judge", "Bulldog", "Guardian", "Phantom" },
                            new KeyboardButton[] { "Vandal", "Marshal", "Operator", "Ares" },
                            new KeyboardButton[] { "Odin", "Холодное оружие" }
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup MapsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Split", "Haven", "Ascent" },
                            new KeyboardButton[] { "Breeze", "Bind", "Icebox" },
                            new KeyboardButton[] { "Fracture", "Pearl", "Стрельбище" },
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup AgentsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Astra", "Breach", "Brimstone" },
                            new KeyboardButton[] { "Chamber", "Cypher", "Fade" },
                            new KeyboardButton[] { "Jett", "KAY/O", "Killjoy" },
                            new KeyboardButton[] { "Neon", "Omen", "Phoenix" },
                            new KeyboardButton[] { "Raze", "Reyna", "Sage" },
                            new KeyboardButton[] { "Skye", "Sova", "Viper" },
                            new KeyboardButton[] { "Yoru" }
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup MenuButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                (
                    new[]
                    {
                        new KeyboardButton[] { "Agents", "Maps" },
                        new KeyboardButton[] { "Weapons", "Skins" },
                        new KeyboardButton[] { "Favourite Agents", "News"},
                        new KeyboardButton[] { "Tournaments" }
                    }
                )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup FavAgentsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Show Agents"},
                            new KeyboardButton[] { "Add Agent"},
                            new KeyboardButton[] { "Delete Agent" }
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private ReplyKeyboardMarkup FavMapsButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Show Maps"},
                            new KeyboardButton[] { "Add Map"},
                            new KeyboardButton[] { "Delete Map"}
                        }
                    )
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private async Task ShowAbilities(ITelegramBotClient botClient, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[] { "Passive", "Ability 1", "Ability 2" },
                            new KeyboardButton[] { "Ability 3", "Ultimate" }
                        }
                    )
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(message.Chat.Id, "Press If You Want To See Abilities",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
