﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TeamBuilder.Data;
using TeamBuilder.ImportExport;
using TeamBuilder.Models;

namespace TeamBuilder.Client.Core.Services
{
    class TeamService
    {
        public static void AddTeams(List<Team> teams)
        {
            using (var db = new TeamBuilderContext())
            {
                db.Teams.AddOrUpdate(t => t.Name, teams.ToArray());

                db.SaveChanges();
            }
        }

        public static void KickMemberFromTeam(string teamName, string username)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                var team = db.Teams.FirstOrDefault(t => t.Name == teamName);
                var user = db.Users.FirstOrDefault(u => u.Username == username);

                db.Users.Attach(user);

                team.Members.Remove(user);
                db.SaveChanges();
            }
        }

        public static void AddTeam(string teamName, string acronym, string description)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                var currentUser = AuthenticationManager.GetCurrentUser();
                db.Users.Attach(currentUser);

                var team = new Team()
                {
                    Name = teamName,
                    Description = description,
                    Acronym = acronym,
                    Creator = currentUser
                };


                db.Teams.Add(team);
                currentUser.Teams.Add(team);

                db.SaveChanges();
            }
        }

        public static bool IsCreatorOrPartOfTeam(string teamName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                User currentUser = AuthenticationManager.GetCurrentUser();

                return db.Teams.Include("Member")
                    .Any(
                        t => t.Name == teamName &&
                             (t.CreatorId == currentUser.Id ||
                              t.Members.Any(m => m.Username == currentUser.Username))
                    );
            }
        }

        public static void DisbandTeam(string teamName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                var team = db.Teams.FirstOrDefault(t => t.Name == teamName);

                db.Teams.Remove(team);
                db.SaveChanges();
            }
        }

        public static string ShowDetails(string teamName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                var team = db.Teams.FirstOrDefault(t => t.Name == teamName);
                string result = $"{teamName} {team.Acronym}\n";

                if (team.Members.Any())
                {
                    result += "Members:";
                }

                foreach (var teamMember in team.Members)
                {
                    result += $"\n--{teamMember.Username}";
                }

                return result;
            }
        }

        public static Team GetTeam(string teamName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Teams
                    .Include("Members")
                    .FirstOrDefault(t => t.Name == teamName);
            }
        }

        public static void ExportTeam(Team team)
        {
            using (var db = new TeamBuilderContext())
            {
                //anonymos obj
                var customersJson = JsonConvert.SerializeObject(team, Formatting.Indented);

                System.IO.File.WriteAllText($"../../bin/Export/team_{team.Name}.json", customersJson);
            }
        }
    }
}