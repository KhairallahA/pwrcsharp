using System;
using Newtonsoft.Json;
namespace PWRCS.Models;

public class JoinTxn : Transaction
{
    [JsonProperty("validator")]
    public string Validator {get;}
   
    public JoinTxn(uint size, ulong blockNumber, uint positionintheBlock, ulong fee, string type, string sender, string receiver, uint nonce, string hash,ulong value,ulong timestamp,string validator)
     : base(size, blockNumber, positionintheBlock, fee, type, sender, receiver, nonce, hash,value, timestamp)
    {
        this.Validator = validator;
    }

    public override string ToString()
        {
            return $"Transaction: Size={Size}, BlockNumber={BlockNumber}, PositionintheBlock={PositionintheBlock}, Fee={Fee}, Type={Type}, Sender={Sender}, Receiver={Receiver}, Nonce={Nonce}, Hash={Hash}, Value={Value}, TimeStamp={TimeStamp}, Validator={Validator}";
        }
}