﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Json;

namespace ConsoleApplication {

	public class Library {
		private List<Book> books;

		public Library() { }

		public void setLibrary(List<Book> books) {
			this.books = books;
		}
	}

	public class Book {
		private string title;
		private string asin;
		private string narrator;
		private string author;

		public Book() { }

		public void setBook(string title, string asin, string narrator, string author) {
			this.title = title;
			this.asin = asin;
			this.narrator = narrator;
			this.author = author;
		}
	}

	public class LoreBooks
	{
		public Library getLibraryFromJson(string jsonStr) {
			Library library = new Library();
			MemoryStream stream = new MemoryStream();
			// DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Library));
			// ser.WriteObject(stream, library);
			return library;
		}

		public static async Task<string> getRequestToken() {
			using(var client = new HttpClient())
			{
				var data = new Dictionary<string, string> {
					{ "grant_type", "refresh_token" },
					{ "client_id", "amzn1.application-oa2-client.4dd3a3f110a3423c93bb12eaa787dcb2" },
					{ "client_secret", "101875b18ad44965b2fa8717160aba7e9905e80ca1c2876fa75ebeebc48daf5e" },
					{ "refresh_token", "Atzr|IwEBIPjiTsGOsJbQmAUhyNa0IG-7-j6svCTNudPfI-BM0gYXCFwXj2tTUVZsqKrnqk4ns3dECZUAK1sKbT4c5PrzgCC6neH22E6usvUG10eA4FwIMDaEhUvKqGkcA8ep3uBpW2YRO1PWpULwZoh4orrLxaQh-Js1EP0JReJKw1lJROiyAIu52Uwko23L8ZrYZ8eZ4GvggnjSI5VnZLCYhXqTrN-wC9_oVHrwv7uUT34hBbt-BIkn7va7MrDfwqyo91qB5bTxnmozUvRhBhiHH1VCBycwVP1L6bCSE7bRgsqPoX27K5Wq4K5riU0Qs0PfFlZLyo_7RAQa0hiY3JGidLu7W_lojbf_UdDkzVw4Zp-GHO9RLmZ_6bkQ343zUiUyHx98mOpWQpgJDyiiVn0TDcM9Q_uukxkxM7YIigH3x8ELnV7zmak_aCqelkK_bmV9-PDEmTtlt7fPd_rkH5UGr8eWA_f09dGsVrVgu7Ag4RdiGAoBFKtKyfSJv8MegHhQZgyc8Ng9negtFDaTuIHS76Up3wmO"}
				};

				var content = new FormUrlEncodedContent(data);
				var response = await client.PostAsync("https://api.amazon.com/auth/o2/token", content);
				var responseString = await response.Content.ReadAsStringAsync();
				Dictionary<string, string>[] responseDict = JsonConvert.DeserializeObject<Dictionary<string,string>[]>(responseString);
				request_token = responseDict["access_token"];
				return request_token;
			}
		}

		public static async Task<string> getLibrary(string auth_token) {
			using(var client = new HttpClient())
			{
				var data = new Dictionary<string, string> {
					{ "Authorization", "bearer " + auth_token},
					{ "Client-ID", "amzn1.application-oa2-client.4dd3a3f110a3423c93bb12eaa787dcb2" },
					{ "Accept", "application/json" },
					{ "Content-Type", "application/json"}
				};

				var content = new FormUrlEncodedContent(data);
				var response = await client.PostAsync("https://api.audible.com/0.0/library/books?purchaseAfterDate=01/01/1990", content);
				var libraryJsonString = await response.Content.ReadAsStringAsync();
				JsonValue value = JsonValue.parse(libraryJsonString.parse);
				result = value as JsonObject;
				return result["access_token"];
			}
		}

		public static void Main(string[] args) {
			string request_token = getRequestToken().Result;
			Console.WriteLine(request_token);
			// Console.WriteLine("");
			// string library_json = getLibrary(request_token).Result;
			// Console.WriteLine(library_json);
		}
	}
}
