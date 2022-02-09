import base64
import datetime
from urllib.parse import urlencode
import requests
import json
import sys


class SpotifyAPI(object):
    access_token = None
    access_token_expires = datetime.datetime.now()
    access_token_did_expire = True
    client_id = None
    client_secret = None
    token_url = "https://accounts.spotify.com/api/token"
    
    def __init__(self, client_id, client_secret, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.client_id = client_id
        self.client_secret = client_secret

    def get_client_credentials(self):
        """
        Returns a base64 encoded string
        """
        client_id = self.client_id
        client_secret = self.client_secret
        if client_secret == None or client_id == None:
            raise Exception("You must set client_id and client_secret")
        client_creds = f"{client_id}:{client_secret}"
        client_creds_b64 = base64.b64encode(client_creds.encode())
        return client_creds_b64.decode()
    
    def get_token_headers(self):
        client_creds_b64 = self.get_client_credentials()
        return {
            "Authorization": f"Basic {client_creds_b64}"
        }
    
    def get_token_data(self):
        return {
            "grant_type": "client_credentials"
        } 
    
    def perform_auth(self):
        token_url = self.token_url
        token_data = self.get_token_data()
        token_headers = self.get_token_headers()
        r = requests.post(token_url, data=token_data, headers=token_headers)
        if r.status_code not in range(200, 299):
            raise Exception("Could not authenticate client.")
            # return False
        data = r.json()
        now = datetime.datetime.now()
        access_token = data['access_token']
        expires_in = data['expires_in'] # seconds
        expires = now + datetime.timedelta(seconds=expires_in)
        self.access_token = access_token
        self.access_token_expires = expires
        self.access_token_did_expire = expires < now
        return True
    
    def get_access_token(self):
        token = self.access_token
        expires = self.access_token_expires
        now = datetime.datetime.now()
        if expires < now:
            self.perform_auth()
            return self.get_access_token()
        elif token == None:
            self.perform_auth()
            return self.get_access_token() 
        return token
    
    def get_resource_header(self):
        access_token = self.get_access_token()
        headers = {
            "Authorization": f"Bearer {access_token}"
        }
        return headers
        
        
    def get_resource(self, lookup_id = None, resource_type='albums', version='v1', addition=''):
        endpoint = f"https://api.spotify.com/{version}/{resource_type}/{lookup_id}{addition}"
        headers = self.get_resource_header()
        r = requests.get(endpoint, headers=headers)
        print(r)
        if r.status_code not in range(200, 299):
            return {}
        return r.json()
    
    def get_albums(self, _id):
        json_data = json.dumps(self.get_resource(_id, resource_type='artists', addition='/albums'))
        dict_data = json.loads(json_data)
        if not dict_data["items"]:
            return None
        else:
            albums = []
            for item in dict_data["items"]:
                album = {
                    'name'        : item["name"],
                    'release_date': item["release_date"],
                    'total_tracks': item["total_tracks"]
                }
                albums.append(album)
            return albums
        
    def get_artist(self, _id):
        return self.get_resource(_id, resource_type='artists')

    def get_track_info(self, _id):
        info_data = self.get_resource(_id, resource_type='tracks')
        features_data = self.get_resource(_id, resource_type='audio-features')

        track_DTO = {
            'artist'    :info_data["artists"][0]["name"],
            'name'      :info_data["name"],
            'audioFeatures':
            {
                'durationMS'    :info_data["duration_ms"],
                'energy'        :features_data["energy"],
                'danceability'  :features_data["danceability"],
                'speechiness'   :features_data["speechiness"],
                'valence'       :features_data["valence"],
                'tempo'         :features_data["tempo"]  
            } 
        }

        return track_DTO
    
    def get_artists(self):
        return self.get_resource(resource_type='artists')
    
    def search(self, query, search_type='artist' ): # type
        headers = self.get_resource_header()
        endpoint = "https://api.spotify.com/v1/search"
        data = urlencode({"q": query, "type": search_type.lower()})
        lookup_url = f"{endpoint}?{data}"
        r = requests.get(lookup_url, headers=headers)
        if r.status_code not in range(200, 299):  
            return {}
        return r.json()


    def get_artist_info(self, artist_name):
        json_data = json.dumps(self.search(artist_name))
        dict_data = json.loads(json_data)
        if not dict_data["artists"]["items"]:
            return None
        else:
            items = dict_data["artists"]["items"][0]
            artist_DTO = {
                'id'     : items["id"],
                'genres' : items["genres"],
                'image'  : items["images"][1]['url']
            }
            return artist_DTO

    
    
if __name__ == "__main__":
    CLIENT_ID = '3854cdee614241c0bd0eff6370e12ec7'
    CLIENT_SECRET = '46d5973b00844a6188f7fc83c81f9dc7'
    spotify = SpotifyAPI(CLIENT_ID, CLIENT_SECRET)

    print(spotify.get_artist_info(sys.argv[1]))
    #tracks = ['7AiMnJSODcJoKDejQ3mnoJ', '73C4vh7W8u41Vll5HvBqv7', '5kYZbBLAGrrhFKNbOs6D95', '18z6OV5lknJmKnZi7aA1zH', '1gHtbcRP4tz1O1NsxPpBea', '3kJudfRjZMItdFYVCCaSi6', '6mzaCRuLTRiz1caGOum3zT', '5h4Uqkh9RpRZwm5ADLh5uj', '3rGew9pmFEmGD9nZ12F1tN', '0dYDmow4l5hbPs5E6QLMSC', '1B3jkf6CyuiF8CQcKlUx9y', '6DkmFhzJrkVhDlcgcEy7Pc', '5dKy6Cgv6xwiRY3j3AJ7Uq', '0oz4ZqHuUaz3uEkP2vD0u8', '389hKTL3ZBPPWP3VuXfEyv', '5cw9s2zGrbny2M2p3WRmGm', '3YaMX9Cf68dxiG6RKo0pSY', '1cAL4sFzXXRMbpZnTPa7Zi', '4w5BVeKJFCj2rrrEy31s0n', '4ta2AWru6ldjg1aHzww0aK', '45DJ0PbKPdbslnyrcM80HN', '7yNu82yd6dYmGQ0H1q0jKo', '4v4HwTfMPslhWAnJxIXchn', '6XptjfnUvLfejptpjPRhCT', '1y2SK8EjL3WSnJvJEMWOoq', '4UMp46x46Zmu9OEr8m3Gl2', '7nb50hgKYhnHJLHKZ7qiKO','5y5OzukBTl0yTRMEdNmApJ', '3Hdl3BEFb1IEbL0Jq53enx', '0lsC0OkBgiLYbSsoHOzMnr', '0Pm1BZp4MpoMKkNxIXCfAu', '2droOB3xlZkhgfUM0owDTq', '2Nd2HLWrIq1DcNMiYPTQUC', '4j644tViOFAf4i0BYT12R8', '4k6hX9RKD096K1NCjjJZLc', '0aSW5EMeNnQSMJQ8QN3zIW', '0tkmYNfaEaH9HpR59ApRtE', '0kas95RruYRVqrOb07rgkh', '4BOikd4oZjOYMde9AXfrTo', '6ZRuF2n1CQxyxxAAWsKJOy', '1EGrDTfEuAiRzRdxlblpET', '32s2Dn9EVvO2f85MrpRoBV', '4jM3c9KLTO9iZPm9A7neiL', '6GCRnf1W9OKxok9fvNp3pz', '1Zm9qGPQkTAOBiVpGSnJUq', '3ucRKbRlikYHyoI17gfR0c', '6HUO25AttZZCoKAY0vUVtc', '5Qr7StTFbXhgHt9JlqJx0I', '1QzC4y8h6WFxHE4KlokhVr', '1xz905v9g71heS0BQQM9re', '5QTdOvIF2ehBMZpSIIGzIo', '7IJiDYPZy2AIJn3YVHhvD4', '23wuZgeX1oyJ43QYOTo9s7', '0tBBihoEWiWKqsO5ZlCbwS']
    