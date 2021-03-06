﻿/*
Copyright(c) 2015, Alysson Ribeiro da Silva All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met :

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and / or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace SharpHoning
{
    class CardAbility
    {
        public CardAbility()
        {
            target = new List<string>();
        }

        public String ability;
        public List<String> target;
        public String value;
    }

    class CardObject
    {
        public CardObject(){
        }

        public String type;
        public String id;
        public String name;
        public String text;
        public String cost;
        public String attack;
        public String health;
        public String hero;
        public String race;
        //public bool collectible;

        public List<CardAbility> abilities;
    }

    class HSCardsParser
    {
        public Dictionary<String, List<CardObject>> objects;

        public HSCardsParser()
        {
            objects = new Dictionary<string, List<CardObject>>();
        }


        public void GetAllTypes()
        {
            Dictionary<String, String> l = new Dictionary<String, String>();
            
            foreach(String key in objects.Keys)
            {
                foreach(CardObject c in objects[key]){
                    if(!l.ContainsKey(c.type)){
                        l.Add(c.type, c.type);
                        }
                }
            }

            string lines = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter("types");

            foreach(String key in l.Keys)
            {
                lines += l[key] + "\n";
            }

            file.WriteLine(lines);

            file.Close();
        }

        public void GetAllTargets()
        {
            Dictionary<String, String> l = new Dictionary<String, String>();

            foreach (String key in objects.Keys)
            {
                foreach (CardObject c in objects[key])
                {
                    foreach (CardAbility a in c.abilities)
                    {
                        foreach(String t in a.target)
                        {
                            if(!l.ContainsKey(t))
                                l.Add(t, t);
                        }
                    }
                }
            }

            string lines = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter("Targets");

            foreach (String key in l.Keys)
            {
                lines += l[key] + "\n";
            }

            file.WriteLine(lines);

            file.Close();
        }

        public void GetAllAbilities()
        {
            Dictionary<String, String> l = new Dictionary<String, String>();

            foreach (String key in objects.Keys)
            {
                foreach (CardObject c in objects[key])
                {
                    foreach (CardAbility a in c.abilities)
                    {
                        if (!l.ContainsKey(a.ability))
                            l.Add(a.ability, a.ability);
                    }
                }
            }

            string lines = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter("Abilities");

            foreach (String key in l.Keys)
            {
                lines += l[key] + "\n";
            }

            file.WriteLine(lines);

            file.Close();
        }

        public HSCardsParser(String path)
        {
            objects = new Dictionary<string, List<CardObject>>();

            // Le arquivo json e retorna string inteira da rede
            String json = System.IO.File.ReadAllText(path);

            // Ler json com dados da rede
            dynamic stuff = JObject.Parse(json);
            foreach (dynamic cardSet in stuff)
                {
                    foreach (dynamic card in cardSet)
                    {
                       // bool collectible = false;
                        dynamic abValue = null;
                        try
                        {
                           // collectible = card.collectible;
                            abValue = card.abilities;
                            // do stuff with x
                        }
                        catch (RuntimeBinderException)
                        {
                            //  MyProperty doesn't exist
                        }

                        //if (collectible == false)
                        //    continue;

                        List<CardAbility> abilities = new List<CardAbility>();

                        if (abValue!=null)
                        {
                            foreach (dynamic ability in card.abilities)
                            {
                                CardAbility cardAb = new CardAbility();
                                cardAb.ability = ability.ability;
                                cardAb.value = ability.value;

                                foreach (String t in ability.target)
                                    cardAb.target.Add(t);

                                abilities.Add(cardAb);
                            }
                        }

                        String race = card.race;
                        String type = card.type;
                        String id = card.id;
                        String name = card.name;
                        String text = card.text;
                        String cost = card.cost;
                        String attack = card.attack;
                        String health = card.health;
                        String hero = card.playerClass;

                        if (hero == null)
                            hero = "Neutral";

                        if (!objects.ContainsKey(hero))
                            objects.Add(hero, new List<CardObject>());

                        List<CardObject> l = objects[hero];

                        CardObject newobj = new CardObject();
                        newobj.type = type;
                        newobj.race = race;
                        newobj.id = id;
                        newobj.name = name;
                        newobj.text = text;
                        newobj.cost = cost;
                        newobj.attack = attack;
                        newobj.health = health;
                        newobj.hero = hero;
                        newobj.abilities = abilities;

                        //---------------- To Lowernormalization
                        if(newobj.type!=null)
                            newobj.type = newobj.type.ToLower();
                        if (newobj.race != null)
                            newobj.race = newobj.race.ToLower();
                        if (newobj.id != null)
                            newobj.id= newobj.id.ToLower();
                        if (newobj.name != null)
                            newobj.name = newobj.name.ToLower();
                        if (newobj.text != null)
                            newobj.text = newobj.text.ToLower();
                        if (newobj.cost != null)
                            newobj.cost = newobj.cost.ToLower();
                        if (newobj.attack != null)
                            newobj.attack = newobj.attack.ToLower();
                        if (newobj.health != null)
                            newobj.health = newobj.health.ToLower();
                        if (newobj.hero != null)
                            newobj.hero = newobj.hero.ToLower();
                        foreach (CardAbility c in newobj.abilities)
                        {
                            c.ability = c.ability.ToLower();
                            c.value = c.value.ToLower();
                            for (int i = 0; i < c.target.Count; i++)
                                c.target[i] = c.target[i].ToLower();
                        }
                        //----------------- To Lowernormalization

                        l.Add(newobj);

                    }// end card
                }//end card set
        }// end constructor
    }   
}
