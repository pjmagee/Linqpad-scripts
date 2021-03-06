<Query Kind="Expression" />

/// <summary> 
/// Drops all M# Meta Databases except for the latest M# meta database that was created from the M# meta files.
/// -*- mode: c#;-*-
/// </summary>

(from db in Sysdatabases.AsEnumerable()
let match = Regex.Match(db.Name, @"^@Meta_(.*)\s\((.+)\)$")
where match.Success
let time = match.Groups[2].Value
orderby time descending
group match.Value by match.Groups[1].Value into g
let oldMeta = g.Skip(1)
where oldMeta.Any()
select oldMeta)
.SelectMany(g => g)
.Select(db => String.Format("Dropping {0} (result: {1})", db, ExecuteCommand(String.Format("DROP DATABASE [{0}]", db))))
