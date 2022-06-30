using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ValorantTGBot.Models;

namespace ValorantTGBot
{
    public class ApiMethods
    {
        private HttpClient _httpClient;
        private static string _address;

        public ApiMethods()
        {
            _address = Constants.Address;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_address);
        }

        public async Task<AgentsModel> AgentInfo(string agentName)
        {
            var response = await _httpClient.GetAsync($"/Valorant/Agents?agentName={agentName}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<AgentsModel>(content);

            return result;
        }

        public async Task<MapsModel> MapInfo(string mapName)
        {
            var response = await _httpClient.GetAsync($"/Valorant/Maps?mapName={mapName}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<MapsModel>(content);

            return result;
        }

        public async Task<WeaponsModel> WeaponInfo(string weaponName)
        {
            var response = await _httpClient.GetAsync($"/Valorant/Weapons?weaponName={weaponName}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<WeaponsModel>(content);

            return result;
        }

        public async Task<SkinsModel> WeaponSkins(string weaponName)
        {
            var response = await _httpClient.GetAsync($"/Valorant/Skins?weaponName={weaponName}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<SkinsModel>(content);

            return result;
        }

        public async Task<List<AgentFavModel>> ShowFavouriteAgents()
        {
            var response = await _httpClient.GetAsync("/FavouriteAgents/FavAgents");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<List<AgentFavModel>>(content);

            return result;
        }

        public async Task<AgentFavModel> AddFavouriteAgent(string agentName)
        {
            var response = await _httpClient.PostAsync($"/FavouriteAgents/AddFavAgent?agentName={agentName}", null);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<AgentFavModel>(content);

                return result;
            }
        }

        public async Task<AgentFavModel> DeleteFavouriteAgent(string agentName)
        {
            var response = await _httpClient.DeleteAsync($"/FavouriteAgents/DeleteAgent?agentName={agentName}");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<AgentFavModel>(content);

                return result;
            }
        }

        public async Task<List<MapsModel>> ShowFavouriteMaps()
        {
            var response = await _httpClient.GetAsync("/FavouritesMaps/FavMaps");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<List<MapsModel>>(content);

            return result;
        }

        public async Task<MapsModel> AddFavouriteMap(string mapName)
        {
            var response = await _httpClient.PostAsync($"/FavouritesMaps/AddFavMap?mapName={mapName}", null);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<MapsModel>(content);

                return result;
            }
        }

        public async Task<MapsModel> DeleteFavouriteMap(string mapName)
        {
            var response = await _httpClient.DeleteAsync($"/FavouritesMaps/DeleteMap?mapName={mapName}");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<MapsModel>(content);

                return result;
            }
        }

        public async Task<string> GetNews()
        {
            var response = await _httpClient.GetAsync("/ValorantNews/ValorantNews");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;

                if (result.Length > 4095)
                {
                    return null;
                }
                else
                {
                    return result;
                }
            }
        }

        public async Task<string> GetTournaments()
        {
            var response = await _httpClient.GetAsync("/ValorantNews/ValorantTournaments");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
        }
    }
}
