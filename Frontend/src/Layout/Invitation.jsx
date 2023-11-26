import camera from "../assets/camera-analytics.jpg";
import { LabelDestinationLinkButton } from "../Components/Buttons/CustomizedButtons";

const Invitation = () => {
  return (
    <div className="w-full bg-white py-16 px-4  text-black">
      <div className="max-w-[1240px] mx-auto grid md:grid-cols-2 gap-2">
        <img className="w-[500px] mx-auto my-4" src={camera} alt="camera" />
        <div className="flex flex-col justify-center">
          <h1 className="text-[#9F86C0] md:text-4xl sm:text-3xl text-2xl font-bold py-2">
            JOIN US
          </h1>
          <p>
            AND BECOME THE PHOTOGRAPHER OF THE YEAR. Lorem, ipsum dolor sit amet
            consectetur adipisicing elit. Dolorem iste quidem sed quisquam
            nostrum laudantium similique perspiciatis facilis ipsam dolorum!
          </p>
          <LabelDestinationLinkButton
            nav={false}
            label="Get Started"
            destination="/register"
          />
        </div>
      </div>
    </div>
  );
};

export default Invitation;
