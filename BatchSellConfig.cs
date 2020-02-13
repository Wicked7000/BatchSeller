using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BatchSeller
{
    public class BatchSellConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Print the result of the batch sell into chat")]
        public bool printSellSummary;

        [DefaultValue(false)]
        [Label("Restricts batch selling to items with the same prexix. (e.g Godly, Demonic...etc)")]
        public bool onlySellMatchingPrefix;
    }
}
