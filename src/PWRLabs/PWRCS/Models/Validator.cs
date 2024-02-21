﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PWRCS.Models;

public class Validator
{
    [JsonProperty("address")]

    public string Address { get; }
    [JsonProperty("ip")]

    public string Ip { get; }
    [JsonProperty("badActor")]

    public bool BadActor { get; }
    [JsonProperty("votingPower")]

    public decimal VotingPower { get; }
    [JsonProperty("shares")]

    public decimal Shares { get; }
    [JsonProperty("delegatorsCount")]

    public int DelegatorsCount { get; }
    [JsonProperty("status")]

    public string Status { get; }
    
    private readonly HttpClient _httpClient;

    public Validator(string address, string ip, bool badActor, decimal votingPower, decimal shares, int delegatorsCount, string status, HttpClient httpClient)
    {
        Address = address;
        Ip = ip;
        BadActor = badActor;
        VotingPower = votingPower;
        Shares = shares;
        DelegatorsCount = delegatorsCount;
        Status = status;
        _httpClient = httpClient;
    }

    public async Task<List<Delegator>> GetDelegators(string rpcNodeUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{rpcNodeUrl}/validator/delegatorsOfValidator/?validatorAddress={Address}");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<JObject>(responseString);
                var delegatorsData = (JObject)data["delegators"];
                var delegatorsList = new List<Delegator>();

                foreach (var kvp in delegatorsData)
                {
                    var delegatorAddress = "0x" + kvp.Key;
                    var shares = kvp.Value.Value<decimal>();
                    var delegatedPwr = shares * VotingPower;
                    var delegator = new Delegator(delegatorAddress, Address, shares, delegatedPwr);
                    delegatorsList.Add(delegator);
                }

                return delegatorsList;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorData = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                throw new Exception($"Failed with HTTP error 400 and message: {errorData["message"]}");
            }
            else
            {
                throw new Exception($"Failed with HTTP error code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException httpErr)
        {
            throw new Exception($"HTTP error occurred: {httpErr.Message}");
        }
        catch (Exception err)
        {
            throw new Exception($"An error occurred: {err.Message}");
        }
    }
}