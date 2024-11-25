using NBitcoin;
using Sake;
using WalletWasabi.Extensions;

var results = new List<SimulationResult>();

var inputCount = 200;
var userCount = 80;
var allowedOutputTypes = new List<ScriptType> { ScriptType.Taproot, ScriptType.P2WPKH };

var min = Money.Satoshis(5000m);
var max = Money.Coins(43000m);
var feeRate = new FeeRate(10m);
var random = new Random();

var maxInputCost = Money.Satoshis(Math.Max(NBitcoinExtensions.P2wpkhInputVirtualSize, NBitcoinExtensions.P2trInputVirtualSize) * feeRate.SatoshiPerByte);


var preRandomAmounts = Sample.Amounts
        .Where(x => Money.Coins(x) > maxInputCost)
        .RandomElements(inputCount)
        .Select(x => new Input(Money.Coins(x), allowedOutputTypes.RandomElement(random), feeRate, 0L));


var mixer = new Mixer(feeRate, min, max, allowedOutputTypes, random);

var groups = mixer.RandomInputGroups(preRandomAmounts, userCount);
var outputGroups = mixer.CompleteMix(groups).ToArray();

foreach (var group in outputGroups) {
    foreach (var output in group) {
        Console.Write(output);
        Console.Write(" ");
    }
    Console.WriteLine();
}
