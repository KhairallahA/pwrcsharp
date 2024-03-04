using System;
using Newtonsoft.Json;
namespace PWRCS.Models;

public class ClaimSpotTxn : Transaction
{
    [JsonProperty("vmId")]
    public string Validator {get;}
    public ClaimSpotTxn(uint size, ulong blockNumber, uint positionintheBlock, ulong fee, string type, string sender, string receiver, uint nonce, string hash,ulong value, ulong timestamp,string validator)
     : base(size, blockNumber, positionintheBlock, fee, type, sender, receiver, nonce, hash,value, timestamp)
    {
        this.Validator = validator;
    }
}