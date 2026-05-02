using SpyFall.Server.Models;

namespace SpyFall.Server.Data;

public static class DatabaseSeeder
{
	public static async Task SeedAsync(AppDbContext db)
	{
		if (db.Locations.Any())
			return;

		var locations = new List<Location>
		{
			new() {
				Name = "Beach",
				Roles =
				[
					new () { Name = "Lifeguard" },
					new () { Name = "Surfer" },
					new () { Name = "Tourist" },
					new () { Name = "Volleyball Player" },
					new () { Name = "Ice Cream Vendor" },
					new () { Name = "Photographer" },
					new () { Name = "Scuba Diver" },
				]
			},
			new() {
				Name = "Space Station",
				Roles =
				[
					new () { Name = "Astronaut" },
					new () { Name = "Engineer" },
					new () { Name = "Scientist" },
					new () { Name = "Commander" },
					new () { Name = "Medical Officer" },
					new () { Name = "Pilot" },
					new () { Name = "Robot" },
				]
			},
			new() {
				Name = "Hospital",
				Roles =
				[
					new () { Name = "Doctor" },
					new () { Name = "Nurse" },
					new () { Name = "Patient" },
					new () { Name = "Surgeon" },
					new () { Name = "Anesthesiologist" },
					new () { Name = "Receptionist" },
					new () { Name = "Janitor" },
				]
			},
			new() {
				Name = "Police Station",
				Roles =
				[
					new () { Name = "Detective" },
					new () { Name = "Patrol Officer" },
					new () { Name = "Suspect" },
					new () { Name = "Desk Sergeant" },
					new () { Name = "Forensic Technician" },
					new () { Name = "Lawyer" },
					new () { Name = "Informant" },
				]
			},
			new() {
				Name = "Restaurant",
				Roles =
				[
					new () { Name = "Chef" },
					new () { Name = "Waiter" },
					new () { Name = "Customer" },
					new () { Name = "Sommelier" },
					new () { Name = "Busboy" },
					new () { Name = "Food Critic" },
					new () { Name = "Manager" },
				]
			},
			new() {
				Name = "Hotel",
				Roles =
				[
					new () { Name = "Receptionist" },
					new () { Name = "Concierge" },
					new () { Name = "Guest" },
					new () { Name = "Housekeeper" },
					new () { Name = "Security Guard" },
					new () { Name = "Bellhop" },
					new () { Name = "Manager" },
				]
			},
			new() {
				Name = "School",
				Roles =
				[
					new () { Name = "Teacher" },
					new () { Name = "Student" },
					new () { Name = "Principal" },
					new () { Name = "Janitor" },
					new () { Name = "Cafeteria Worker" },
					new () { Name = "Librarian" },
					new () { Name = "Coach" },
				]
			},
			new() {
				Name = "Airplane",
				Roles =
				[
					new () { Name = "Pilot" },
					new () { Name = "Co-Pilot" },
					new () { Name = "Flight Attendant" },
					new () { Name = "Passenger" },
					new () { Name = "Air Marshal" },
					new () { Name = "Mechanic" },
					new () { Name = "First Class Passenger" },
				]
			},
			new() {
				Name = "Casino",
				Roles =
				[
					new () { Name = "Dealer" },
					new () { Name = "Gambler" },
					new () { Name = "Security Guard" },
					new () { Name = "Pit Boss" },
					new () { Name = "Cocktail Waitress" },
					new () { Name = "Cheater" },
					new () { Name = "High Roller" },
				]
			},
			new() {
				Name = "Military Base",
				Roles =
				[
					new () { Name = "General" },
					new () { Name = "Soldier" },
					new () { Name = "Medic" },
					new () { Name = "Sniper" },
					new () { Name = "Cook" },
					new () { Name = "Engineer" },
					new () { Name = "Intelligence Officer" },
				]
			},
			new() {
				Name = "Pirate Ship",
				Roles =
				[
					new () { Name = "Captain" },
					new () { Name = "First Mate" },
					new () { Name = "Navigator" },
					new () { Name = "Cannoneer" },
					new () { Name = "Cook" },
					new () { Name = "Captive" },
					new () { Name = "Lookout" },
				]
			},
			new() {
				Name = "Supermarket",
				Roles =
				[
					new () { Name = "Cashier" },
					new () { Name = "Stock Clerk" },
					new () { Name = "Shopper" },
					new () { Name = "Store Manager" },
					new () { Name = "Security Guard" },
					new () { Name = "Butcher" },
					new () { Name = "Delivery Driver" },
				]
			},
			new() {
				Name = "Movie Studio",
				Roles =
				[
					new () { Name = "Director" },
					new () { Name = "Actor" },
					new () { Name = "Cameraman" },
					new () { Name = "Stuntman" },
					new () { Name = "Makeup Artist" },
					new () { Name = "Producer" },
					new () { Name = "Extra" },
				]
			},
			new() {
				Name = "Circus",
				Roles =
				[
					new () { Name = "Clown" },
					new () { Name = "Acrobat" },
					new () { Name = "Ringmaster" },
					new () { Name = "Tightrope Walker" },
					new () { Name = "Animal Trainer" },
					new () { Name = "Magician" },
					new () { Name = "Spectator" },
				]
			},
			new() {
				Name = "Submarine",
				Roles =
				[
					new () { Name = "Captain" },
					new () { Name = "Navigator" },
					new () { Name = "Engineer" },
					new () { Name = "Sonar Operator" },
					new () { Name = "Cook" },
					new () { Name = "Torpedo Technician" },
					new () { Name = "Medic" },
				]
			},
			new() {
				Name = "Banquet Hall",
				Roles =
				[
					new () { Name = "Host" },
					new () { Name = "Guest of Honor" },
					new () { Name = "Caterer" },
					new () { Name = "Bartender" },
					new () { Name = "Event Planner" },
					new () { Name = "Waiter" },
					new () { Name = "Gate Crasher" },
				]
			},
			new() {
				Name = "Antarctic Research Station",
				Roles =
				[
					new () { Name = "Biologist" },
					new () { Name = "Meteorologist" },
					new () { Name = "Station Commander" },
					new () { Name = "Doctor" },
					new () { Name = "Cook" },
					new () { Name = "Engineer" },
					new () { Name = "Explorer" },
				]
			},
			new() {
				Name = "Art Museum",
				Roles =
				[
					new () { Name = "Curator" },
					new () { Name = "Security Guard" },
					new () { Name = "Visitor" },
					new () { Name = "Art Restorer" },
					new () { Name = "Tour Guide" },
					new () { Name = "Thief" },
					new () { Name = "Journalist" },
				]
			},
			new() {
				Name = "Bank",
				Roles =
				[
					new () { Name = "Manager" },
					new () { Name = "Teller" },
					new () { Name = "Customer" },
					new () { Name = "Security Guard" },
					new () { Name = "Loan Officer" },
					new () { Name = "Robber" },
					new () { Name = "Armored Car Driver" },
				]
			},
			new() {
				Name = "Broadway Theater",
				Roles =
				[
					new () { Name = "Actor" },
					new () { Name = "Director" },
					new () { Name = "Audience Member" },
					new () { Name = "Stagehand" },
					new () { Name = "Costume Designer" },
					new () { Name = "Critic" },
					new () { Name = "Understudy" },
				]
			},
			new() {
				Name = "Cathedral",
				Roles =
				[
					new () { Name = "Priest" },
					new () { Name = "Parishioner" },
					new () { Name = "Choir Member" },
					new () { Name = "Tourist" },
					new () { Name = "Organist" },
					new () { Name = "Wedding Planner" },
					new () { Name = "Groundskeeper" },
				]
			},
			new() {
				Name = "Coal Mine",
				Roles =
				[
					new () { Name = "Miner" },
					new () { Name = "Foreman" },
					new () { Name = "Engineer" },
					new () { Name = "Safety Inspector" },
					new () { Name = "Explosives Expert" },
					new () { Name = "Cart Operator" },
					new () { Name = "Medic" },
				]
			},
			new() {
				Name = "Corporate Party",
				Roles =
				[
					new () { Name = "CEO" },
					new () { Name = "Intern" },
					new () { Name = "Accountant" },
					new () { Name = "HR Manager" },
					new () { Name = "Bartender" },
					new () { Name = "IT Guy" },
					new () { Name = "Salesperson" },
				]
			},
			new() {
				Name = "Crusader Army",
				Roles =
				[
					new () { Name = "Knight" },
					new () { Name = "Archer" },
					new () { Name = "Priest" },
					new () { Name = "Squire" },
					new () { Name = "Commander" },
					new () { Name = "Spy" },
					new () { Name = "Cook" },
				]
			},
			new() {
				Name = "Day Spa",
				Roles =
				[
					new () { Name = "Masseuse" },
					new () { Name = "Customer" },
					new () { Name = "Receptionist" },
					new () { Name = "Nail Technician" },
					new () { Name = "Esthetician" },
					new () { Name = "Manager" },
					new () { Name = "Yoga Instructor" },
				]
			},
			new() {
				Name = "Embassy",
				Roles =
				[
					new () { Name = "Ambassador" },
					new () { Name = "Consul" },
					new () { Name = "Security Officer" },
					new () { Name = "Visa Applicant" },
					new () { Name = "Translator" },
					new () { Name = "Secretary" },
					new () { Name = "Spy" },
				]
			},
			new() {
				Name = "Jail",
				Roles =
				[
					new () { Name = "Warden" },
					new () { Name = "Guard" },
					new () { Name = "Prisoner" },
					new () { Name = "Lawyer" },
					new () { Name = "Chaplain" },
					new () { Name = "Psychologist" },
					new () { Name = "New Inmate" },
				]
			},
			new() {
				Name = "Night Club",
				Roles =
				[
					new () { Name = "DJ" },
					new () { Name = "Bouncer" },
					new () { Name = "Bartender" },
					new () { Name = "VIP Guest" },
					new () { Name = "Dancer" },
					new () { Name = "Coat Check" },
					new () { Name = "Regular" },
				]
			},
			new() {
				Name = "Ocean Liner",
				Roles =
				[
					new () { Name = "Captain" },
					new () { Name = "Passenger" },
					new () { Name = "Chef" },
					new () { Name = "Entertainer" },
					new () { Name = "Engineer" },
					new () { Name = "Waiter" },
					new () { Name = "Stowaway" },
				]
			},
			new() {
				Name = "Passenger Train",
				Roles =
				[
					new () { Name = "Conductor" },
					new () { Name = "Passenger" },
					new () { Name = "Engineer" },
					new () { Name = "Ticket Inspector" },
					new () { Name = "Dining Car Waiter" },
					new () { Name = "Sleeper Car Attendant" },
					new () { Name = "Stowaway" },
				]
			},
			new() {
				Name = "Polar Station",
				Roles =
				[
					new () { Name = "Researcher" },
					new () { Name = "Pilot" },
					new () { Name = "Doctor" },
					new () { Name = "Cook" },
					new () { Name = "Radio Operator" },
					new () { Name = "Geologist" },
					new () { Name = "Supply Manager" },
				]
			},
			new() {
				Name = "Racing Track",
				Roles =
				[
					new () { Name = "Driver" },
					new () { Name = "Pit Crew" },
					new () { Name = "Commentator" },
					new () { Name = "Spectator" },
					new () { Name = "Race Official" },
					new () { Name = "Mechanic" },
					new () { Name = "Sponsor Rep" },
				]
			},
			new() {
				Name = "Service Station",
				Roles =
				[
					new () { Name = "Mechanic" },
					new () { Name = "Cashier" },
					new () { Name = "Customer" },
					new () { Name = "Manager" },
					new () { Name = "Car Wash Attendant" },
					new () { Name = "Tow Truck Driver" },
					new () { Name = "Inspector" },
				]
			},
			new() {
				Name = "Soccer Game",
				Roles =
				[
					new () { Name = "Player" },
					new () { Name = "Referee" },
					new () { Name = "Coach" },
					new () { Name = "Fan" },
					new () { Name = "Commentator" },
					new () { Name = "Medic" },
					new () { Name = "Ball Boy" },
				]
			},
			new() {
				Name = "Spy HQ",
				Roles =
				[
					new () { Name = "Director" },
					new () { Name = "Field Agent" },
					new () { Name = "Analyst" },
					new () { Name = "Tech Specialist" },
					new () { Name = "Double Agent" },
					new () { Name = "Receptionist" },
					new () { Name = "Recruiter" },
				]
			},
			new() {
				Name = "Stadium",
				Roles =
				[
					new () { Name = "Athlete" },
					new () { Name = "Coach" },
					new () { Name = "Referee" },
					new () { Name = "Spectator" },
					new () { Name = "Announcer" },
					new () { Name = "Vendor" },
					new () { Name = "Security Guard" },
				]
			},
			new() {
				Name = "University",
				Roles =
				[
					new () { Name = "Professor" },
					new () { Name = "Student" },
					new () { Name = "Dean" },
					new () { Name = "Janitor" },
					new () { Name = "Librarian" },
					new () { Name = "Teaching Assistant" },
					new () { Name = "Security Guard" },
				]
			},
			new() {
				Name = "Vineyard",
				Roles =
				[
					new () { Name = "Winemaker" },
					new () { Name = "Tour Guide" },
					new () { Name = "Visitor" },
					new () { Name = "Grape Picker" },
					new () { Name = "Sommelier" },
					new () { Name = "Owner" },
					new () { Name = "Delivery Driver" },
				]
			},
			new() {
				Name = "Wedding",
				Roles =
				[
					new () { Name = "Bride" },
					new () { Name = "Groom" },
					new () { Name = "Best Man" },
					new () { Name = "Maid of Honor" },
					new () { Name = "Priest" },
					new () { Name = "Wedding Photographer" },
					new () { Name = "Guest" },
				]
			},
		};

		db.Locations.AddRange(locations);
		await db.SaveChangesAsync();
	}
}
