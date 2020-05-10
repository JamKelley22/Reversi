import axios from "axios";

const URI = "https://localhost:44348";

const CREATE_GAME = async (): Promise<any> => {
  return await axios
    .post(`${URI}/reversiboardgame`, {
      firstName: "Fred",
      lastName: "Flintstone"
    })
    .then(function(response) {
      console.log(response);
    })
    .catch(function(error) {
      console.log(error);
    });
};

const GET_GAME = async (id: number): Promise<any> => {
  return await axios
    .get(`${URI}/reversiboardgame`)
    .then(function(response) {
      console.log(response);
    })
    .catch(function(error) {
      console.log(error);
    });
};

// const GET = async (endpoint: string, params: string[]): Promise<any> => {
//   if (endpoint.length === 0) endpoint = "/";
//   if (endpoint[0] !== "/") endpoint = "/" + endpoint;
//     let paramString = (params.length > 0) ? "?" : "";
//     params.forEach((param: string, i: number) => {
//         paramString += `${}=${}`
//     });
//   let res = await fetch(endpoint + paramString);
//   let data;
//   try {
//     data = await res.json();
//   } catch (e) {
//     return res;
//   }
//   return data;
// };

export { CREATE_GAME };
