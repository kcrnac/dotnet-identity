type RuquestCard = {
  requestType: string;
  title: string;
  description: string;
  buttonText: string;
  request: () => Promise<any>;
};

export type { RuquestCard as RuquestCardType };
